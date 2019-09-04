using System.Collections.Generic;
using System.Threading.Tasks;
using FESA.SCM.Core.Models;
using FESA.SCM.Core.Services.Interfaces;
using Microsoft.Practices.ServiceLocation;

namespace FESA.SCM.Core.ViewModels
{
    public class ReportVm : ViewModelBase
    {
        private readonly IReportService _reportService;
        private readonly IIdentityService _identityService;

        public List<Ocupability> OcupabilityLevels { get; set; }
        public List<OcupabilityThroughTime> UserOcupabilityThroughTime { get; set; }
        public List<OrdersPerClient> OrdersPerClient { get; set; }
        public List<Assignment> AssignmentsPerClient { get; set; }
        public Enumerations.UserStatus SelectedStatus { get; set; }
        public List<User> Users { get; set; }
        public User SelectedUser { get; set; }

        public ReportVm()
        {
            _reportService = ServiceLocator.Current.GetInstance<IReportService>();
            _identityService = ServiceLocator.Current.GetInstance<IIdentityService>();
        }

        public async Task GetUsersPerStateAsync()
        {
            Users = await _reportService.GetUsersForStateAsync(SelectedStatus);
        }

        public async Task GetSubordinateOcupabilityLevelAsync()
        {
            var user = await _identityService.GetUserDataAsync();
            OcupabilityLevels = await _reportService.GetOcupabilityLevelAsync(user.Id);
        }

        public async Task GetTechnicianOcupalityThroughTimeAsync()
        {
            UserOcupabilityThroughTime = await _reportService.GetOcupabilityThroughTimePerUserAsync(SelectedUser.Id);
        }

        public async Task GetOrdersPerClientAsync()
        {
            var user = await _identityService.GetUserDataAsync();
            OrdersPerClient = await _reportService.GetOrdersPerClientAsync(user.Id);
        }
    }
}