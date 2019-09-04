using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FESA.SCM.Core.Models;

namespace FESA.SCM.Core.Services.Interfaces
{
    public interface IReportService
    {
        Task<List<Ocupability>> GetOcupabilityLevelAsync(string supervisorId, CancellationToken cancellationToken = default(CancellationToken));

        Task<List<OcupabilityThroughTime>> GetOcupabilityThroughTimePerUserAsync(string technicianId,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<List<OrdersPerClient>> GetOrdersPerClientAsync(string supervisorId, CancellationToken cancellationToken = default(CancellationToken));

        Task<List<User>> GetUsersForStateAsync(Enumerations.UserStatus status, CancellationToken cancellationToken = default(CancellationToken));
    }
}