using System;
using System.Linq;
using CoreGraphics;
using FESA.SCM.Core;
using FESA.SCM.Core.Helpers;
using FESA.SCM.Core.Models;
using FESA.SCM.Core.ViewModels;
using FESA.SCM.iPhone.Controls;
using FESA.SCM.iPhone.Helpers;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using UIKit;

namespace FESA.SCM.iPhone.Controllers
{
    public class RestorePasswordViewController : BaseController
    {
        private TextField _userName;
        private Button _restorePassword;
        private readonly RestorePasswordVm _restorePasswordVm;

        public override bool HandlesKeyboardNotifications => true;

        public RestorePasswordViewController()
        {
            _restorePasswordVm = ServiceLocator.Current.GetInstance<RestorePasswordVm>();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            BuildInterface();
            _restorePasswordVm.IsBusyChanged += IsBusyChanged;
            _restorePasswordVm.IsValidChanged += IsValidChanged;
        }

        private void BuildInterface()
        {
            View.BackgroundColor = new UIImage("Images/fondoInicio.jpg").GetScaledImageBackground(View);
            
            nfloat height = 40f;
            var width = View.Bounds.Width;

            var logo = new UIImageView(new UIImage("Images/logo.jpg"))
            {
                Frame = new CGRect(60, 40, 200, 45)
            };

            var sendIcon = new UIImageView(new UIImage("Images/btn-enviando.png"))
            {
                Frame = new CGRect(120, logo.Frame.Y + logo.Frame.Height + 80, 80, 100)
            };

            _userName = new TextField
            {
                KeyboardType = UIKeyboardType.EmailAddress,
                Placeholder = "Correo Electrónico",
                RightImage = "Images/icon-user.png",
                Shape = new CGRect(0, 315, width, height)
            };
            _userName.SetDidChangeNotification(text => _restorePasswordVm.UserName = text.Text);
            _userName.ShouldReturn = condition =>
            {
                if (_restorePasswordVm.IsValid)
                    RestorePassword();
                else
                {
                    new UIAlertView(title: "Ferreyros", message: _restorePasswordVm.Error, del: null,
                            cancelButtonTitle: "OK", otherButtons: null).Show();
                }
                return true;
            };

            var message = new UILabel
            {
                Frame = new CGRect(50, _userName.Frame.Y + _userName.Frame.Height + 10, 200, 80),
                Font = UIFont.FromName("Helvetica-Light", 15f),
                TextColor = UIColor.White,
                TextAlignment = UITextAlignment.Center,
                LineBreakMode = UILineBreakMode.WordWrap,
                Lines = 5,
                Text = "Le enviaremos una nueva contraseña a su correo electrónico, gracias."
                
            };

            _restorePassword = new Button(UIButtonType.RoundedRect)
            {
                Content = "ENVIAR",
                Frame = new CGRect(20, message.Frame.Y + message.Frame.Height + Constants.TextboxMargin, View.Frame.Width - 40,
                    Constants.ButtonHeight)

            };
            _restorePassword.TouchUpInside += Submit;

            var copyRight = new UILabel
            {
                Text = "2016 Ferreyros | Derechos Reservados",
                TextAlignment = UITextAlignment.Center,
                TextColor = UIColor.White,
                Font = UIFont.FromName("Helvetica-Light", 10f),
                Frame = new CGRect(0, View.Bounds.Height - 30, width, 20)
            };

            View.AddSubviews(logo, sendIcon, _userName, _restorePassword, message, copyRight);
        }

        private void Submit(object sender, EventArgs e)
        {
            if (!_restorePasswordVm.IsValid)
            {
                new UIAlertView(title: "Ferreyros", message: _restorePasswordVm.Error, del: null,
                        cancelButtonTitle: "OK", otherButtons: null).Show();
                return;
            }
            RestorePassword();
        }

        private async void RestorePassword()
        {
            _userName.ResignFirstResponder();

            var restored = await _restorePasswordVm.RestorePasswordAsync();
            if (!restored)
            {
                new UIAlertView(title: "Ferreyros", message: _restorePasswordVm.Error, del: null,
                    cancelButtonTitle: "OK", otherButtons: null).Show();
                return;
            }
            var nav = ServiceLocator.Current.GetInstance<INavigationService>();
            nav.GoBack();
        }

        private void IsBusyChanged(object sender, EventArgs eventArgs)
        {
            var bounds = UIScreen.MainScreen.Bounds;
            var overlay = View.Subviews.Last() as LoadingOverlay ?? new LoadingOverlay(bounds);

            if (_restorePasswordVm.IsBusy)
                View.AddSubview(overlay);
            else
                overlay.Hide();
        }

        protected override void OnKeyboardChanged(bool visible, nfloat height)
        {
            var frame = View.Frame;

            if (visible)
                frame.Y -= height / 5f;
            else
                frame.Y += height / 5f;
            View.Frame = frame;
        }
        private void IsValidChanged(object sender, EventArgs e)
        {

        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _restorePasswordVm.IsBusyChanged -= IsBusyChanged;
            _restorePasswordVm.IsValidChanged -= IsValidChanged;
        }

    }
}