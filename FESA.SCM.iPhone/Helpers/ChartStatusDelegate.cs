using FESA.SCM.Core.Models;
using FESA.SCM.Core.ViewModels;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using Syncfusion.SfChart.iOS;

namespace FESA.SCM.iPhone.Helpers
{
    public class ChartStatusDelegate : SFChartDelegate
    {
        private readonly ReportVm _reportsVm;
        private readonly INavigationService _navigationService;
        public ChartStatusDelegate()
        {
            _reportsVm = ServiceLocator.Current.GetInstance<ReportVm>();
            _navigationService = ServiceLocator.Current.GetInstance<INavigationService>();
        }
        public override void DidDataPointSelect(SFChart chart, SFChartSelectionInfo info)
        {
            if(info.SelectedDataPointIndex != -1)
                _reportsVm.SelectedStatus = _reportsVm.OcupabilityLevels[info.SelectedDataPointIndex].UserStatus;
            _navigationService.NavigateTo(Constants.PageKeys.UsersPerState);
        }
    }
}