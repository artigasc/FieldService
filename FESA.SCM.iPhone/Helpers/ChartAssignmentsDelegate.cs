using System.Linq;
using FESA.SCM.Core.Models;
using FESA.SCM.Core.ViewModels;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using Syncfusion.SfChart.iOS;

namespace FESA.SCM.iPhone.Helpers
{
    public class ChartAssignmentsDelegate : SFChartDelegate
    {
        private readonly AssignmentVm _assignmentVm;
        private readonly ReportVm _reportVm;
        private readonly INavigationService _navigationService;
        public ChartAssignmentsDelegate()
        {
            _assignmentVm = ServiceLocator.Current.GetInstance<AssignmentVm>();
            _reportVm = ServiceLocator.Current.GetInstance<ReportVm>();
            _navigationService = ServiceLocator.Current.GetInstance<INavigationService>();
        }
        public override void DidDataPointSelect(SFChart chart, SFChartSelectionInfo info)
        {
            if (info.SelectedDataPointIndex != -1)
                _assignmentVm.SelectedCompanyName =
                    _reportVm.OrdersPerClient.Where(x => x.AssignmentType == Enumerations.AssignmentType.FieldService)
                        .ToList()[info.SelectedDataPointIndex].CompanyName;
            _navigationService.NavigateTo(Constants.PageKeys.AssignmentsPerClient);
        }
    }
}