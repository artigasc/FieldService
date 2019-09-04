using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FESA.SCM.Core.Models;

namespace FESA.SCM.Core.Services.Interfaces
{
    public interface IAssignmentService
    {
        Task<List<Assignment>> GetAssignmentsAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task<Activity> GetActiveActivityAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task<string> SaveAssignmentAsync(Assignment assignment, CancellationToken cancellationToken = default(CancellationToken));

        Task<List<Assignment>> GetAssignmentHistoryAsync(
            CancellationToken cancellationToken = default(CancellationToken));

        Task<Assignment> GetAssignmentAsync(string assignmentId,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<string> SaveActivityAsync(Activity activity,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<string> SaveDocumentsAsync(List<Document> documents, string assignmentId,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<List<Activity>> GetActivitiesByAssignmentAsync(string assignmentId,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<List<Document>> GetDocumentsByAssignmentAsync(string assignmentId,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<int> GetAssignmentCountAsync(CancellationToken cancellationToken = default(CancellationToken));

        Task<int> GetHistoryAssignmentCountAsync(CancellationToken cancellationToken = default(CancellationToken));

        Task<bool> SyncAssignmentsAsync(List<Assignment> assignments,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<List<Assignment>> FetchAllAssingmentsAsync(
            CancellationToken cancellationToken = default(CancellationToken));

        Task<bool> SendAssignmentCompletionToClientAsync(string assignmentId,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<bool> SendActivityMailToClientAsync(string assignmentId, Enumerations.ActivityType activityType,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<List<Assignment>> GetAssignmentByCompanyAsync(string companyName,
            CancellationToken cancellationToken = default(CancellationToken));

        Task SaveTraceByActivityAsync(List<Trace> traces,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}