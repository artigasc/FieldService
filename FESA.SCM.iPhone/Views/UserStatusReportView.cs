using CoreGraphics;
using FESA.SCM.Core.ViewModels;
using FESA.SCM.iPhone.Controls;
using FESA.SCM.iPhone.Helpers;
using Foundation;
using Microsoft.Practices.ServiceLocation;
using Syncfusion.SfChart.iOS;
using UIKit;

namespace FESA.SCM.iPhone.Views
{
    public class UserStatusReportView : UIView
    {
        private readonly ReportVm _reportsVm;
        private readonly CGRect _bounds;
        public UserStatusReportView(CGRect bounds)
        {
            _reportsVm = ServiceLocator.Current.GetInstance<ReportVm>();
            _bounds = bounds;
            BuildInterface();
        }

        private async void BuildInterface()
        {
            await _reportsVm.GetSubordinateOcupabilityLevelAsync();
            Frame = new CGRect(0, 0, _bounds.Width, _bounds.Height);
            var chart = new SFChart();
            chart.Title.Text = new NSString("Ocupabilidad (%)");
            chart.Legend.Visible = true;
            chart.Delegate = new ChartStatusDelegate();
            chart.DataSource = new OcupabilityDataSource();
            chart.Frame = new CGRect(0, 20, _bounds.Width, _bounds.Height - 20);
            AddSubviews(chart);
        }
    }
}