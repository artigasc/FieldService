using System;
using CoreGraphics;
using FESA.SCM.Core.Models;
using FESA.SCM.Core.Services.Interfaces;
using FESA.SCM.iPhone.Controls;
using FESA.SCM.iPhone.Helpers;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using UIKit;

namespace FESA.SCM.iPhone.Views
{
    public class HomeLayoutView : UIView
    {
        private readonly CGRect _bounds;
        private Header _header;
        private UIView _background;
        private UIView _content;
        private readonly INavigationService _navigationService;
        private readonly ISessionService _sessionService;

        public EventHandler MenuButtonTouched
        {
            set { _header.ButtonTouch = value; }
        }
        public HomeLayoutView(CGRect bounds)
        {
            _bounds = bounds;
            _navigationService = ServiceLocator.Current.GetInstance<INavigationService>();
            _sessionService = ServiceLocator.Current.GetInstance<ISessionService>();
            BuildInterface();
        }

        private void BuildInterface()
        {
            Bounds = _bounds;
            Frame = new CGRect(0, 0, _bounds.Width, _bounds.Height);
            BackgroundColor = new UIImage("Images/fondoPrincipal.jpg").GetScaledImageBackground(this);
            UserInteractionEnabled = true;
            _header = new Header(_bounds.Width) {LeftButtonImage = "Images/menu.png"};

            _background = new UIView
            {
                Frame =
                    new CGRect(0, _header.Frame.Y + _header.Frame.Height, Frame.Width,
                        Frame.Height - _header.Frame.Height - 40),
                BackgroundColor = UIColor.White
            };

            _content = new UIView(_background.Frame);
            SelectContent(menuIndex: 0);

            var copyRight = new UILabel
            {
                Text = "2016 Ferreyros | Derechos Reservados",
                TextAlignment = UITextAlignment.Center,
                TextColor = UIColor.White,
                Font = UIFont.FromName("Helvetica-Light", 10f),
                Frame = new CGRect(0, Frame.Height - 30, Frame.Width, 20)
            };

            AddSubviews(_header, _background ,copyRight);
        }

        public void SelectContent(int menuIndex)
        {
            foreach (var subview in Subviews)
            {
                if(subview.Equals(_content))
                    subview.RemoveFromSuperview();
            }
            
            var session = _sessionService.GetFromSession("technician");
            _content.RemoveFromSuperview();
            _content = new UIView(_background.Frame);
            switch (menuIndex)
            {
                case 0:
                    if (!string.IsNullOrEmpty(session))
                    {
                        _header.LocationTitle = "Servicios";
                        _content = new AssignmentsView(_background.Frame);
                    }
                    else
                    {
                        _header.LocationTitle = "Ocupabilidad";
                        _content = new UserStatusReportView(_background.Frame);
                    }
                    break;
                case 1:
                    if (!string.IsNullOrEmpty(session))
                    {
                        _header.LocationTitle = "Historial de Servicios";
                        _content = new AssignmentHistoryView(_background.Frame);
                    }
                    else
                    {
                        _header.LocationTitle = "OT / Clientes";
                        _content = new CompanyWorkReportView(_background.Frame);
                    }
                    break;
                case 2:
                    _sessionService.Logout();
                    _navigationService.NavigateTo(Constants.PageKeys.Login);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();    
            }
            _background.AddSubview(_content);
            _header.PerformOuterTouch();
        }

    }
}