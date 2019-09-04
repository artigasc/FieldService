using System;
using System.Linq;
using CoreGraphics;
using FESA.SCM.Core.Models;
using FESA.SCM.Core.Services.Interfaces;
using FESA.SCM.Core.ViewModels;
using FESA.SCM.iPhone.Controls;
using FESA.SCM.iPhone.Helpers;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using UIKit;

namespace FESA.SCM.iPhone.Controllers
{
    public class ChangePasswordViewController : BaseController
    {
        private TextField _password;
        private TextField _passwordConfirm;
        private Button _changePassword;
        private readonly ChangePasswordVm _changePasswordVm;
        private readonly INavigationService _navigationService;

        public override bool HandlesKeyboardNotifications => true;

        public ChangePasswordViewController()
        {
            _changePasswordVm = ServiceLocator.Current.GetInstance<ChangePasswordVm>();
            _navigationService = ServiceLocator.Current.GetInstance<INavigationService>();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            BuildInterface();
            _changePasswordVm.IsBusyChanged += IsBusyChanged;
            _changePasswordVm.IsValidChanged += IsValidChanged;
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

            _password = new TextField
            {
                Placeholder = "Contraseña",
                SecureTextEntry = true,
                Shape = new CGRect(0, 290, width, height),
                ImageTouch = ShowPasswordTouch,
                AfterImageTouch = HidePasswordTouch,
                RightImage = "Images/btn-ver-contra.png",
            };
            _password.SetDidChangeNotification(text => _changePasswordVm.Password = text.Text);
            _password.ShouldReturn = condition =>
            {
                _passwordConfirm.BecomeFirstResponder();
                return false;
            };

            _passwordConfirm = new TextField
            {
                Placeholder = "Confirme Contraseña",
                SecureTextEntry = true,
                After = _password,
                ImageTouch = ShowPasswordTouch,
                AfterImageTouch = HidePasswordTouch,
                RightImage = "Images/btn-ver-contra.png",
            };
            _passwordConfirm.SetDidChangeNotification(text => _changePasswordVm.PasswordConfirm = text.Text);
            _passwordConfirm.ShouldReturn = condition =>
            {
                if (!_changePasswordVm.IsValid)
                {
                    new UIAlertView(title: "Ferreyros", message: _changePasswordVm.Error, del: null,
                            cancelButtonTitle: "OK", otherButtons: null).Show();
                    return false;
                }
                ChangePassword();
               
                return false;
            };


            _changePassword = new Button(UIButtonType.RoundedRect)
            {
                Content = "ENVIAR",
                After = _passwordConfirm
            };
            _changePassword.TouchUpInside += Submit;

            var copyRight = new UILabel
            {
                Text = "2016 Ferreyros | Derechos Reservados",
                TextAlignment = UITextAlignment.Center,
                TextColor = UIColor.White,
                Font = UIFont.FromName("Helvetica-Light", 10f),
                Frame = new CGRect(0, View.Bounds.Height - 30, width, 20)
            };

            View.AddSubviews(logo, appName, _password, _passwordConfirm, _changePassword, copyRight);
        }


        private void ShowPasswordTouch(object sender, EventArgs eventArgs)
        {
            var button = (UIButton) sender;
            var passwordField = (UITextField)button.Superview;
            var firstResponder = passwordField.IsFirstResponder;
            if (firstResponder) passwordField.ResignFirstResponder();
            passwordField.SecureTextEntry = false;
            if (firstResponder) passwordField.BecomeFirstResponder();

        }
        private void HidePasswordTouch(object sender, EventArgs eventArgs)
        {
            var button = (UIButton) sender;
            var passwordField = (UITextField)button.Superview;
            var firstResponder = passwordField.IsFirstResponder;
            if (firstResponder) passwordField.ResignFirstResponder();
            passwordField.SecureTextEntry = true;
            if (firstResponder) passwordField.BecomeFirstResponder();

        }

        private void Submit(object sender, EventArgs e)
        {
            if (_changePasswordVm.IsValid)
            {
                new UIAlertView(title: "Ferreyros", message: _changePasswordVm.Error, del: null,
                        cancelButtonTitle: "OK", otherButtons: null).Show();
                return;
            }
            ChangePassword();
        }
        
        private async void ChangePassword()
        {
            _password.ResignFirstResponder();
            _passwordConfirm.ResignFirstResponder();

            var changed = await _changePasswordVm.ChangePasswordAsync();
            if (!changed)
            {
                new UIAlertView(title: "Ferreyros", message: _changePasswordVm.Error, del: null,
                    cancelButtonTitle: "OK", otherButtons: null).Show();
                return;
            }
            ServiceLocator.Current.GetInstance<ISessionService>().AddToSession("logged", true);
            _navigationService.NavigateTo(Constants.PageKeys.Home);
        }
        private void IsBusyChanged(object sender, EventArgs eventArgs)
        {
            var bounds = UIScreen.MainScreen.Bounds;
            var overlay = View.Subviews.Last() as LoadingOverlay ?? new LoadingOverlay(bounds);

            if (_changePasswordVm.IsBusy)
                View.AddSubview(overlay);
            else
                overlay.Hide();
        }

        protected override void OnKeyboardChanged(bool visible, nfloat height)
        {
            var frame = View.Frame;

            if (visible)
                frame.Y -= height/5f;
            else
                frame.Y += (height/5f)*2f;
            View.Frame = frame;
        }
        private void IsValidChanged(object sender, EventArgs e)
        {
            if (_password.IsFirstResponder)
                ((UIButton)_password.RightView).Enabled = _changePasswordVm.Password.Length > 0;

            if (_passwordConfirm.IsFirstResponder)
                ((UIButton)_passwordConfirm.RightView).Enabled = _changePasswordVm.PasswordConfirm.Length > 0;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _changePasswordVm.IsBusyChanged -= IsBusyChanged;
            _changePasswordVm.IsValidChanged -= IsValidChanged;
        }
    }
}