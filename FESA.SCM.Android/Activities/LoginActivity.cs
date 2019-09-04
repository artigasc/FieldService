using System;
using System.ComponentModel;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using FESA.SCM.Core.ViewModels;
using Microsoft.Practices.ServiceLocation;

namespace FESA.SCM.Android.Activities
{
    [Activity(Label = "Servicio de Campo", MainLauncher = true, Icon = "@drawable/logoapp", LaunchMode = LaunchMode.SingleTop)]
    public class LoginActivity : Activity, TextView.IOnEditorActionListener
    {
        private EditText _user, _pass;
        private Button _login;
        private readonly LoginVm _loginVm;

        public LoginActivity()
        {
            _loginVm = ServiceLocator.Current.GetInstance<LoginVm>();
        }

        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Login);
            _user = FindViewById<EditText>(Resource.Id.userName);
            _pass = FindViewById<EditText>(Resource.Id.userPassword);
            _login = FindViewById<Button>(Resource.Id.login);

            _user.SetOnEditorActionListener(this);
            _pass.SetOnEditorActionListener(this);

            _user.TextChanged += (sender, args) => { _loginVm.UserName = _user.Text; };
            _pass.TextChanged += (sender, args) => { _loginVm.Password = _pass.Text; };
            _login.Touch += (sender, args) => Login();

        }

        public bool OnEditorAction(TextView v, ImeAction actionId, KeyEvent e)
        {
            switch (actionId)
            {
                case ImeAction.Go:
                    if (_loginVm.IsValid)
                        Login();
                    return true;
                case ImeAction.Next:
                    _pass.RequestFocus();
                    return true;
                default:
                    return false;
            }
        }

        private async void Login()
        {
            var imm = (InputMethodManager) GetSystemService(InputMethodService);
            imm.HideSoftInputFromInputMethod(_pass.WindowToken, HideSoftInputFlags.NotAlways);

            var logged = await _loginVm.LoginAsync();
            if(logged)
                Toast.MakeText(this, "Logged", ToastLength.Long).Show();
        }
    }
}