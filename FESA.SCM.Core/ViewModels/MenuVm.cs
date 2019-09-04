using System.Threading.Tasks;
using FESA.SCM.Core.Models;
using FESA.SCM.Core.Services.Interfaces;
using Microsoft.Practices.ServiceLocation;

namespace FESA.SCM.Core.ViewModels
{
    public class MenuVm : ViewModelBase
    {
        private readonly IAssignmentService _assignmentService;
        private readonly IIdentityService _identityService;
        public User User { get; set; }

        public int AsignmentCount { get; set; }
        public int HistoryAsignmentCount { get; set; }

        public MenuVm()
        {
            _assignmentService = ServiceLocator.Current.GetInstance<IAssignmentService>();
            _identityService = ServiceLocator.Current.GetInstance<IIdentityService>();
        }

        public async Task LoadData()
        {
            User = await _identityService.GetUserDataAsync();
            AsignmentCount = await _assignmentService.GetAssignmentCountAsync();
            HistoryAsignmentCount = await _assignmentService.GetHistoryAssignmentCountAsync();
        }
        public async Task<User> GetUserDataAsync()
        {
            return await _identityService.GetUserDataAsync();
        }

    }
}