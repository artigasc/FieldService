using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FESA.SCM.Core.Helpers;
using FESA.SCM.Core.Models;
using FESA.SCM.Core.Services.Interfaces;

namespace FESA.SCM.Core.Services.Implementation
{
    public class ReportService : IReportService
    {
        private readonly ApiClient _apiClient;

        public ReportService()
        {
            _apiClient = new ApiClient(Constants.FesaApi.BaseUrl);
        }
        public async Task<List<Ocupability>> GetOcupabilityLevelAsync(string supervisorId, CancellationToken cancellationToken = new CancellationToken())
        {
            _apiClient.AddParameter("supervisorId", supervisorId);
            var response = await _apiClient.ExecuteGet<List<Ocupability>>(Constants.FesaApi.Ocupability);
            _apiClient.CleanParameters();
            return (List<Ocupability>) response.Content;
        }

        public async Task<List<OcupabilityThroughTime>> GetOcupabilityThroughTimePerUserAsync(string technicianId,
            CancellationToken cancellationToken = new CancellationToken())
        {
            _apiClient.AddParameter("technicianId", technicianId);
            var response = await _apiClient.ExecuteGet<List<OcupabilityThroughTime>>(Constants.FesaApi.OcupabilityThroughTime);
            _apiClient.CleanParameters();
            return (List<OcupabilityThroughTime>)response.Content;
        }

        public async Task<List<OrdersPerClient>> GetOrdersPerClientAsync(string supervisorId, CancellationToken cancellationToken = new CancellationToken())
        {
            _apiClient.AddParameter("supervisorId", supervisorId);
            var response = await _apiClient.ExecuteGet<List<OrdersPerClient>>(Constants.FesaApi.OrdersPerClient);
            _apiClient.CleanParameters();
            return (List<OrdersPerClient>)response.Content;
        }

        public async Task<List<User>> GetUsersForStateAsync(Enumerations.UserStatus status, CancellationToken cancellationToken = new CancellationToken())
        {
            _apiClient.AddParameter("status", (int)status);
            var response = await _apiClient.ExecuteGet<List<User>>(Constants.FesaApi.UsersForState);
            _apiClient.CleanParameters();
            return (List<User>)response.Content;
        }
    }
}