using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FESA.SCM.Core.Helpers;
using FESA.SCM.Core.Models;
using FESA.SCM.Core.Services.Interfaces;

namespace FESA.SCM.Core.Services.Implementation
{
    public class AssignmentService : IAssignmentService
    {
        private readonly ApiClient _apiClient;

        public AssignmentService()
        {
            _apiClient = new ApiClient(Constants.FesaApi.BaseUrl);
        }

        public async Task<List<Assignment>> GetAssignmentsAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var assignmentCount = await Database.Connection.Table<Assignment>().CountAsync(cancellationToken);
            if (assignmentCount == 0) return await GetAssingmentsFromServiceAsync(cancellationToken);
            var assignments =
                await
                    Database.Connection.Table<Assignment>()
                        .Where(a => a.Status != Enumerations.AssignmentStatus.Complete)
                        .ToListAsync(cancellationToken);

            foreach (var assignment in assignments)
            {
                assignment.Location = await GetLocationByAssignmentAsync(assignment.Id, cancellationToken);
                assignment.Machine = await GetMachineByAssignmentAsync(assignment.Id, cancellationToken);
                assignment.TechnicalContact = await GetTechnicalContactByAssignmentAsync(assignment.Id, cancellationToken);
                assignment.FerreyrosContact = await GetFesaTeamByAssignmentAsync(assignment.Id, cancellationToken);
            }
            return assignments;
        }

        public Task<Activity> GetActiveActivityAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return Database.Connection.FindAsync<Activity>(a => a.State == Enumerations.ActivityState.Active,
                cancellationToken);
        }

        public async Task<string> SaveAssignmentAsync(Assignment assignment,
            CancellationToken cancellationToken = new CancellationToken())
        {
            await Database.Connection.InsertOrReplaceAsync(assignment, cancellationToken);
            return assignment.Id;
        }

        public async Task<List<Assignment>> GetAssignmentHistoryAsync(
            CancellationToken cancellationToken = new CancellationToken())
        {
            var assignments =
                await
                    Database.Connection.Table<Assignment>()
                        .Where(a => a.Status == Enumerations.AssignmentStatus.Complete)
                        .ToListAsync(cancellationToken);

            foreach (var assignment in assignments)
            {
                assignment.Location = await GetLocationByAssignmentAsync(assignment.Id, cancellationToken);
                assignment.Machine = await GetMachineByAssignmentAsync(assignment.Id, cancellationToken);
                assignment.TechnicalContact = await GetTechnicalContactByAssignmentAsync(assignment.Id, cancellationToken);
                assignment.FerreyrosContact = await GetFesaTeamByAssignmentAsync(assignment.Id, cancellationToken);
            }
            return assignments;
        }

        public async Task<Assignment> GetAssignmentAsync(string assignmentId,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return
                await
                    Database.Connection.Table<Assignment>()
                        .Where(a => a.Id == assignmentId)
                        .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<string> SaveActivityAsync(Activity activity,
            CancellationToken cancellationToken = new CancellationToken())
        {
            await Database.Connection.InsertOrReplaceAsync(activity, cancellationToken);
            return activity.Id;
        }

        public async Task<string> SaveDocumentsAsync(List<Document> documents, string assignmentId, CancellationToken cancellationToken = new CancellationToken())
        {
            await
                Database.Connection.ExecuteAsync(cancellationToken, "DELETE FROM DOCUMENT WHERE ASSIGNMENTID = ?",
                    assignmentId);
            await Database.Connection.InsertAllAsync(documents, cancellationToken);
            return string.Empty;
        }

        public async Task<List<Activity>> GetActivitiesByAssignmentAsync(string assignmentId,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return
                await
                    Database.Connection.Table<Activity>()
                        .Where(a => a.AssignmentId == assignmentId)
                        .ToListAsync(cancellationToken);
        }

        public async Task<List<Document>> GetDocumentsByAssignmentAsync(string assignmentId,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return
                await
                    Database.Connection.Table<Document>()
                        .Where(a => a.AssignmentId == assignmentId)
                        .ToListAsync(cancellationToken);
        }

        public async Task<int> GetAssignmentCountAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return
                await
                    Database.Connection.Table<Assignment>()
                        .Where(a => a.Status != Enumerations.AssignmentStatus.Complete)
                        .CountAsync(cancellationToken);
        }

        public async Task<int> GetHistoryAssignmentCountAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return
                await
                    Database.Connection.Table<Assignment>()
                        .Where(a => a.Status == Enumerations.AssignmentStatus.Complete)
                        .CountAsync(cancellationToken);
        }

        public async Task<bool> SyncAssignmentsAsync(List<Assignment> assignments, CancellationToken cancellationToken = new CancellationToken())
        {
            await GetAssingmentsFromServiceAsync(cancellationToken);
            return await SendAssignmentsToServiceAsync(assignments);
        }

        public async Task<List<Assignment>> FetchAllAssingmentsAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var assignments =
                await
                    Database.Connection.Table<Assignment>()
                        .ToListAsync(cancellationToken);

            foreach (var assignment in assignments)
            {
                assignment.Location = await GetLocationByAssignmentAsync(assignment.Id, cancellationToken);
                assignment.Machine = await GetMachineByAssignmentAsync(assignment.Id, cancellationToken);
                assignment.TechnicalContact = await GetTechnicalContactByAssignmentAsync(assignment.Id, cancellationToken);
                assignment.FerreyrosContact = await GetFesaTeamByAssignmentAsync(assignment.Id, cancellationToken);
                assignment.Activities = await GetActivitiesByAssignmentAsync(assignment.Id, cancellationToken);
                assignment.Documents = await GetDocumentsByAssignmentAsync(assignment.Id, cancellationToken);
            }
            return assignments;
        }

        public async Task<bool> SendAssignmentCompletionToClientAsync(string assignmentId, CancellationToken cancellationToken = new CancellationToken())
        {
            _apiClient.AddParameter("assignmentId", assignmentId);
            var response = await _apiClient.ExecutePost(Constants.FesaApi.AssignmentEnded);
            return response.Status != HttpStatusCode.OK;
        }

        public async Task<bool> SendActivityMailToClientAsync(string assignmentId, Enumerations.ActivityType activityType, CancellationToken cancellationToken = new CancellationToken())
        {
            _apiClient.AddParameter("assignmentId",assignmentId);
            _apiClient.AddParameter("activityType", activityType);
            var response = await _apiClient.ExecutePost(Constants.FesaApi.ActivityNotification);
            return response.Status != HttpStatusCode.OK;
        }

        public async Task<List<Assignment>> GetAssignmentByCompanyAsync(string companyName, CancellationToken cancellationToken = new CancellationToken())
        {
            _apiClient.AddParameter("companyName", companyName);
            var response = await _apiClient.ExecuteGet<List<Assignment>>(Constants.FesaApi.OrdersPerClient);
            _apiClient.CleanParameters();
            return (List<Assignment>)response.Content;
        }

        public async Task SaveTraceByActivityAsync(List<Trace> traces, CancellationToken cancellationToken = new CancellationToken())
        {
            await Database.Connection.InsertOrReplaceAllAsync(traces, cancellationToken);
        }

        private async Task<List<Assignment>> GetAssingmentsFromServiceAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var user = await Database.Connection.Table<User>().FirstAsync(cancellationToken);
            _apiClient.AddParameter("userId", user.Id);
            var response = await _apiClient.ExecuteGet<List<Assignment>>(Constants.FesaApi.GetAssignments);

            if (response.Status != HttpStatusCode.OK)
                return new List<Assignment>();

            var result = (List<Assignment>)response.Content;
            await Database.Connection.InsertOrIgnoreAllAsync(result, cancellationToken);
            foreach (var assignment in result)
            {
                await Database.Connection.InsertOrIgnoreAsync(assignment.Location);
                await Database.Connection.InsertOrIgnoreAsync(assignment.Machine);
                await Database.Connection.InsertOrIgnoreAsync(assignment.FerreyrosContact);
                await Database.Connection.InsertOrIgnoreAsync(assignment.TechnicalContact);
            }
            _apiClient.CleanParameters();
            return result;
        }

        private async Task<bool> SendAssignmentsToServiceAsync(List<Assignment> assignments)
        {
            _apiClient.AddParameter("assignments", assignments);
            var response = await _apiClient.ExecutePost(Constants.FesaApi.SyncAssignments);
            return response.Status != HttpStatusCode.OK;
        }

        private async Task<Machine> GetMachineByAssignmentAsync(string assignmentId,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return await Database.Connection.FindAsync<Machine>(l => l.AssignmentId == assignmentId, cancellationToken);
        }

        private async Task<Location> GetLocationByAssignmentAsync(string assignmentId,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return await Database.Connection.FindAsync<Location>(l => l.AssignmentId == assignmentId, cancellationToken);
        }

        private async Task<Contact> GetTechnicalContactByAssignmentAsync(string assignmentId,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return await
                Database.Connection.FindAsync<Contact>(c => c.AssignmentId == assignmentId && !c.IsFerreyrosContact,
                    cancellationToken);
        }


        private async Task<Contact> GetFesaTeamByAssignmentAsync(string assignmentId,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return await
                Database.Connection.FindAsync<Contact>(c => c.AssignmentId == assignmentId && c.IsFerreyrosContact,
                    cancellationToken);
        }
    }
}