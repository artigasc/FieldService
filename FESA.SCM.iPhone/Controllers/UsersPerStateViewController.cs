using CoreGraphics;
using FESA.SCM.Core.ViewModels;
using FESA.SCM.iPhone.Controls;
using FESA.SCM.iPhone.Helpers;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using UIKit;

namespace FESA.SCM.iPhone.Controllers
{
    public class UsersPerStateViewController : BaseController
    {
        private readonly ReportVm _reportVm;
        private readonly INavigationService _navigationService;
        public UsersPerStateViewController()
        {
            _reportVm = ServiceLocator.Current.GetInstance<ReportVm>();
            _navigationService = ServiceLocator.Current.GetInstance<INavigationService>();
            BuildInterface();
        }

        private async void BuildInterface()
        {
            await _reportVm.GetUsersPerStateAsync();
            View.BackgroundColor = new UIImage("Images/fondoInicio.jpg").GetScaledImageBackground(View);

            var header = new Header(View.Frame.Width)
            {
                LocationTitle = Utils.GetUserStatusText(_reportVm.SelectedStatus),
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

            var userList = new UITableView
            {
                Frame = new CGRect(0, 0, background.Frame.Width, background.Frame.Height),
                BackgroundColor = UIColor.Clear,
                SeparatorColor = UIColor.LightGray,
                Source = new UserTableSource(),
                RowHeight = UITableView.AutomaticDimension,
                EstimatedRowHeight = 60
            };

            background.AddSubviews(userList);

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