using System;
using System.Linq;
using CoreGraphics;
using FESA.SCM.Core.Models;
using FESA.SCM.Core.ViewModels;
using FESA.SCM.iPhone.Controls;
using FESA.SCM.iPhone.Helpers;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using UIKit;

namespace FESA.SCM.iPhone.Controllers
{
    public class LoginViewController : BaseController
    {
        private TextField _userName;
        private TextField _userPassword;
        private readonly LoginVm _loginVm;
        private readonly INavigationService _navigationService;
        private Button _login;

        public override bool HandlesKeyboardNotifications => true;

        public LoginViewController()
        {
            _navigationService = ServiceLocator.Current.GetInstance<INavigationService>();
            _loginVm = ServiceLocator.Current.GetInstance<LoginVm>();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            BuildInterface();
            _loginVm.IsBusyChanged += IsBusyChanged;
            _loginVm.IsValidChanged += IsValidChanged;
        }
        

        private void BuildInterface()
        {
            View.BackgroundColor = new UIImage("Images/fondoInicio.jpg").GetScaledImageBackground(View);

            nfloat height = 40.0f;
            var width = View.Bounds.Width;

            var logo = new UIImageView(new UIImage("Images/logo.jpg"))
            {
                Frame = new CGRect(60, 60, 200, 45)
            };

            var appName = new UIImageView(new UIImage("Images/titulo.png"))
            {
                Frame = new CGRect(30, logo.Frame.Y + logo.Frame.Height + 20, 260, 40)
            };

            _userName = new TextField
            {
                KeyboardType = UIKeyboardType.EmailAddress,
                Placeholder = "Nombre de Usuario",
                RightImage = "Images/icon-user.png",
                Shape = new CGRect(0, 290, width, height)
            };
            _userName.SetDidChangeNotification(text => _loginVm.UserName = text.Text);
            _userName.ShouldReturn = condition =>
            {
                _userPassword.BecomeFirstResponder();
                return false;
            };

            _userPassword = new TextField
            {
                Placeholder = "Contraseña",
                SecureTextEntry = true,
                After = _userName,
                ImageTouch = ShowPasswordTouch,
                AfterImageTouch = HidePasswordTouch,
                RightImage = "Images/btn-ver-contra.png",
            };
            _userPassword.SetDidChangeNotification(text => _loginVm.Password = text.Text);

            _userPassword.ShouldReturn = condition =>
            {
                if (_loginVm.IsValid)
                    Login();
                else
                {
                    new UIAlertView(title: "Ferreyros", message: _loginVm.Error, del: null,
                            cancelButtonTitle: "OK", otherButtons: null).Show();
                }
                return false;
            };


            _login = new Button(UIButtonType.RoundedRect)
            {
                Content = "INGRESAR",
                After = _userPassword

            };
            _login.TouchUpInside += Submit;

            var forgetPasswordButton = new Button(UIButtonType.RoundedRect)
            {
                After = _login,
                HorizontalAlignment = UIControlContentHorizontalAlignment.Left,
                VerticalAlignment = UIControlContentVerticalAlignment.Top,
                Content = "¿Olvidaste tu contraseña?",
                TitleLabel =
                {
                  Font  = UIFont.FromName("Helvetica-Light", 15f)
                },
                TintColor = UIColor.White,
                BackgroundColor = UIColor.Clear
            };
            forgetPasswordButton.TouchUpInside += ForgetPasswordButton_TouchUpInside;


            var copyRight = new UILabel
            {
                Text = "2016 Ferreyros | Derechos Reservados",
                TextAlignment = UITextAlignment.Center,
                TextColor = UIColor.White,
                Font = UIFont.FromName("Helvetica-Light", 10f),
                Frame = new CGRect(0, View.Bounds.Height - 30, width, 20)
            };

            View.AddSubviews(logo, appName, _userName, _userPassword, _login, forgetPasswordButton, copyRight);
        }

        private void ShowPasswordTouch(object sender, EventArgs eventArgs)
        {
            var firstResponder = _userPassword.IsFirstResponder;
            if (firstResponder) _userPassword.ResignFirstResponder();
            _userPassword.SecureTextEntry = false;
            if (firstResponder) _userPassword.BecomeFirstResponder();

        }
        private void HidePasswordTouch(object sender, EventArgs eventArgs)
        {
            var firstResponder = _userPassword.IsFirstResponder;
            if (firstResponder) _userPassword.ResignFirstResponder();
            _userPassword.SecureTextEntry = true;
            if (firstResponder) _userPassword.BecomeFirstResponder();
        }

        private void ForgetPasswordButton_TouchUpInside(object sender, EventArgs e)
        {
            _navigationService.NavigateTo(Constants.PageKeys.RestorePassword);
        }

        private void Submit(object sender, EventArgs e)
        {
            if (!_loginVm.IsValid)
            {
                new UIAlertView(title: "Ferreyros", message: _loginVm.Error, del: null,
                        cancelButtonTitle: "OK", otherButtons: null).Show();
                return;
            }
            if (Reachability.InternetConnectionStatus() == NetworkStatus.NotReachable)
            {
                new UIAlertView(title: "Ferreyros", message: Constants.Messages.NoNetwork, del: null,
                        cancelButtonTitle: "OK", otherButtons: null).Show();
                return;
            }
            Login();
        }

        private async void Login()
        {
            _userName.ResignFirstResponder();
            _userPassword.ResignFirstResponder();

            var user = await _loginVm.LoginAsync();

            if (user == null || user.SessionActive)
            {
                new UIAlertView(title: "Ferreyros", message: _loginVm.Error, del: null,
                    cancelButtonTitle: "OK", otherButtons: null).Show();
                return;
            }
            _navigationService.NavigateTo(!user.ChangedPassword ? Constants.PageKeys.ChangePassword : Constants.PageKeys.Home);
        }

        private void IsValidChanged(object sender, EventArgs e)
        {
            if (_userPassword.IsFirstResponder)
                ((UIButton)_userPassword.RightView).Enabled = _loginVm.Password.Length > 0;
        }

        private void IsBusyChanged(object sender, EventArgs eventArgs)
        {
            var bounds = UIScreen.MainScreen.Bounds;
            var overlay = View.Subviews.Last() as LoadingOverlay ?? new LoadingOverlay(bounds);

            if (_loginVm.IsBusy)
                View.AddSubview(overlay);
            else
                overlay.Hide();
        }

        protected override void OnKeyboardChanged(bool visible, nfloat height)
        {
            var frame = View.Frame;

            var factor = _userPassword.IsFirstResponder ? 2f : 1f;

            if (visible)
                frame.Y -= 40 * factor;
            else
                frame.Y += 40 * factor;
            View.Frame = frame;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _loginVm.IsBusyChanged -= IsBusyChanged;
            _loginVm.IsValidChanged -= IsValidChanged;
        }
    }

}