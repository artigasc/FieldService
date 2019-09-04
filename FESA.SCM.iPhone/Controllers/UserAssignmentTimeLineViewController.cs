using CoreGraphics;
using FESA.SCM.Core.ViewModels;
using FESA.SCM.iPhone.Controls;
using FESA.SCM.iPhone.Helpers;
using Foundation;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using Syncfusion.SfChart.iOS;
using UIKit;

namespace FESA.SCM.iPhone.Controllers
{
    public class UserAssignmentTimeLineViewController : BaseController
    {
        private readonly ReportVm _reportVm;
        private readonly INavigationService _navigationService;

        public UserAssignmentTimeLineViewController()
        {
            _reportVm = ServiceLocator.Current.GetInstance<ReportVm>();
            _navigationService = ServiceLocator.Current.GetInstance<INavigationService>();
            BuildInterface();
        }

        private async void BuildInterface()
        {
            await _reportVm.GetTechnicianOcupalityThroughTimeAsync();
            View.BackgroundColor = new UIImage("Images/fondoInicio.jpg").GetScaledImageBackground(View);

            var header = new Header(View.Frame.Width)
            {
                LocationTitle = _reportVm.SelectedUser.Name,
                LeftButtonImage = "Images/btn-atras.png",
                ButtonTouch = (sender, args) => { _navigationService.GoBack(); }
            };

            var background = new UIView
            {
                Frame =
                    new CGRect(0, header.Frame.Height, View.Frame.Width,
                        View.Frame.Height - header.Frame.Height - 30),
                BackgroundColor = UIColor.White
            };

            var chart = new SFChart { PrimaryAxis = new SFCategoryAxis { Interval = FromObject(1) } };
            chart.PrimaryAxis.Title.Text = new NSString("Meses");
            var yAxis = new SFLogarithmicAxis
            {
                ShowMajorGridLines = true,
                MinorTicksPerInterval = 5
            };
            yAxis.Title.Text = new NSString("Asignaciones");
            chart.SecondaryAxis = yAxis;
            chart.Title.EdgeInsets = new UIEdgeInsets(10, 5, 10, 5);
            chart.EdgeInsets = new UIEdgeInsets(-3, 5, 5, 10);
            chart.Title.Text = new NSString("Ocupabilidad/Tiempo (%)");
            chart.Delegate = new ChartMonthDelegate();
            chart.DataSource = new TimelineDataSource();
            chart.Frame = new CGRect(0, 30, background.Frame.Width, background.Frame.Height - 50);

            background.AddSubviews(chart);

            var copyRight = new UILabel
            {
                Text = "2016 Ferreyros | Derechos Reservados",
                TextAlignment = UITextAlignment.Center,
                TextColor = UIColor.White,
                Font = UIFont.FromName("Helvetica-Light", 10f),
                Frame = new CGRect(0, View.Frame.Height - 30, View.Frame.Width, 20)
            };

            View.AddSubviews(header, background, copyRight);
        }
    }
}