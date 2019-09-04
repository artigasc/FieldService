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
    public class CompanyWorkReportView : UIView
    {
        private readonly ReportVm _reportVm;
        private readonly CGRect _bounds;

        public CompanyWorkReportView(CGRect bounds)
        {
            _reportVm = ServiceLocator.Current.GetInstance<ReportVm>();
            _bounds = bounds;
            BuildInterface();
        }

        private async void BuildInterface()
        {
            await _reportVm.GetOrdersPerClientAsync();
            Frame = new CGRect(0, 0, _bounds.Width, _bounds.Height);
            var chart = new SFChart
            {
                PrimaryAxis = new SFCategoryAxis
                {
                    Title = {Text = new NSString("Clientes")},
                    Interval = new NSNumber(1),
                    LabelPlacement = SFChartLabelPlacement.BetweenTicks
                },
                SecondaryAxis = new SFNumericalAxis()
                {
                    Minimum = new NSNumber(0),
                    Maximum = new NSNumber(100),
                    Interval = new NSNumber(20),
                    EdgeLabelsDrawingMode = SFChartAxisEdgeLabelsDrawingMode.Shift
                }
            };
            chart.SecondaryAxis.Title.Text = new NSString("Número de Ordenes");
            chart.Title.Text = new NSString("Ordenes de Trabajo por Cliente");
            chart.Delegate = new ChartAssignmentsDelegate();
            chart.DataSource = new OrdersDataSource();
            chart.Legend.Visible = true;
            chart.Legend.DockPosition = SFChartLegendPosition.Bottom;
            chart.ColorModel.Palette = SFChartColorPalette.TomatoSpectrum;
            chart.Frame = new CGRect(0, 20, _bounds.Width, _bounds.Height - 20);
            AddSubviews(chart);
        }
    }
}