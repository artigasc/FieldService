using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FESA.SCM.Core.Helpers;
using FESA.SCM.Core.Models;
using FESA.SCM.Core.Services.Interfaces;
using Microsoft.Practices.ServiceLocation;

namespace FESA.SCM.Core.ViewModels
{
    public class AssignmentVm : ViewModelBase
    {
        #region Members
        private readonly IAssignmentService _assignmentService;
        private List<Assignment> _assignments, _assignmentHistory;
        private Assignment _activeAssignment, _selectedAssignment, _lastAssignment;
        private Activity _selectedActivity, _activeActivity;
        private readonly Timer _timer;
        #endregion
        #region Properties
        public event EventHandler RecordingChanged;
        public event EventHandler TimerChanged;

        public Enumerations.ServiceDataType SelectedServiceDataType { get; set; }

        public Assignment LastAssignment
        {
            get { return _lastAssignment; }
            set
            {
                _lastAssignment = value;
                OnPropertyChanged(nameof(LastAssignment));
            }
        }

        public Assignment SelectedAssignment
        {
            get { return _selectedAssignment; }
            set
            {
                _selectedAssignment = value;
                OnPropertyChanged(nameof(SelectedAssignment));
            }
        }

        public Assignment ActiveAssignment
        {
            get { return _activeAssignment; }
            set
            {
                _activeAssignment = value;
                OnPropertyChanged(nameof(ActiveAssignment));
            }
        }
        public List<Assignment> Assignments
        {
            get { return _assignments; }
            set
            {
                _assignments = value;
                OnPropertyChanged(nameof(Assignments));
            }
        }

        public Activity SelectedActivity
        {
            get { return _selectedActivity; }
            set
            {
                _selectedActivity = value;
                OnPropertyChanged(nameof(SelectedActivity));
            }
        }

        public List<Assignment> AssignmentHsitory
        {
            get { return _assignmentHistory; }
            set
            {
                _assignmentHistory = value;
                OnPropertyChanged(nameof(AssignmentHsitory));
            }
        }

        public Activity ActiveActivity
        {
            get { return _activeActivity;}
            set
            {
                _activeActivity = value;
                OnPropertyChanged(nameof(ActiveActivity));
            }
        }
        
        public string SelectedCompanyName { get; set; }
        #endregion
        #region Constructor
        public AssignmentVm()
        {
            _assignmentService = ServiceLocator.Current.GetInstance<IAssignmentService>();
            _timer = new Timer(TimeSpan.FromSeconds(1), () =>
            {
                SelectedActivity.Duration = SelectedActivity.Duration.Add(TimeSpan.FromSeconds(1));
                TimerChanged?.Invoke(this, EventArgs.Empty);
            });
        }
        #endregion
        #region Public Methods
        public async Task LoadAssignmentsAsync()
        {
            var assignmentsTotal = await _assignmentService.GetAssignmentsAsync();
            Assignments = assignmentsTotal.Where(a => a.Status != Enumerations.AssignmentStatus.Complete).ToList();
            ActiveAssignment = assignmentsTotal.FirstOrDefault(x => x.Status == Enumerations.AssignmentStatus.Active);
            AssignmentHsitory = assignmentsTotal.Where(a => a.Status == Enumerations.AssignmentStatus.Complete).ToList();
            ActiveActivity = await _assignmentService.GetActiveActivityAsync();
            if (ActiveActivity != null)
                ActiveActivity.IsRecording = true;
        }

        public async Task LoadHistoryAsync()
        {
            AssignmentHsitory = await _assignmentService.GetAssignmentHistoryAsync();
        }

        public async Task SaveAssignmentAsync()
        {
            await _assignmentService.SaveAssignmentAsync(SelectedAssignment);

            if (SelectedAssignment.Status == Enumerations.AssignmentStatus.Complete)
            {
                ActiveAssignment = null;
            }
        }

        public async Task<string> SaveActivityAsync()
        {
            await _assignmentService.SaveTraceByActivityAsync(SelectedActivity.Traces);
            return await _assignmentService.SaveActivityAsync(SelectedActivity);
        }

        public async Task<string> SaveDocumentsAsync()
        {
            return await _assignmentService.SaveDocumentsAsync(SelectedAssignment.Documents, SelectedAssignment.Id);
        }

        public async Task LoadActivitiesByAssignmentAsync()
        {
            SelectedAssignment.Activities = await _assignmentService.GetActivitiesByAssignmentAsync(SelectedAssignment.Id);
        }

        public async Task LoadDocumentsByAssignmentAsync()
        {
            SelectedAssignment.Documents = await _assignmentService.GetDocumentsByAssignmentAsync(SelectedAssignment.Id);
        }

        public Task RecordAsync()
        {
            if (SelectedActivity == null)
                return Task.Factory.StartNew(delegate { });
            SelectedAssignment.Status = Enumerations.AssignmentStatus.Active;
            SelectedActivity.IsRecording = true;
            if(SelectedActivity.StartDate == DateTime.MinValue)
                SelectedActivity.StartDate = DateTime.Now;
            SelectedActivity.State = Enumerations.ActivityState.Active;
            ActiveActivity = SelectedActivity;
            _timer.Start();
            RecordingChanged?.Invoke(this, EventArgs.Empty);
            return _assignmentService.SaveActivityAsync(SelectedActivity);
        }

        public Task StopAsync(bool willContinue = false)
        {
            if (SelectedActivity == null)
                return Task.Factory.StartNew(delegate { });

            if (!willContinue)
            {
                SelectedActivity.IsRecording = false;
                SelectedActivity.EndDate = DateTime.Now;
                SelectedActivity.State = Enumerations.ActivityState.Completed;
                ActiveActivity = null;
            }
            _timer.Stop();
            RecordingChanged?.Invoke(this, EventArgs.Empty);
            return _assignmentService.SaveActivityAsync(SelectedActivity);
        }

        public async Task SyncAssignments()
        {
            var assignments = await _assignmentService.FetchAllAssingmentsAsync();
            await _assignmentService.SyncAssignmentsAsync(assignments);
        }

        public async Task<bool> SendActivityEndsAssignmentMailAsync()
        {
            return await _assignmentService.SendAssignmentCompletionToClientAsync(SelectedActivity.AssignmentId);
        }

        public async Task<bool> SendActivityMailAsync()
        {
            return
                await
                    _assignmentService.SendActivityMailToClientAsync(SelectedActivity.AssignmentId,
                        SelectedActivity.ActivityType);
        }

        public async Task LoadAssignmentsByClient()
        {
            Assignments = await _assignmentService.GetAssignmentByCompanyAsync(SelectedCompanyName);
        }
        #endregion
    }
}