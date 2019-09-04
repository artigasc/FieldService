using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FESA.SCM.Core.Models;
using FESA.SCM.Core.Services.Interfaces;
using Microsoft.Practices.ServiceLocation;

namespace FESA.SCM.Core.ViewModels
{
    public class LoginVm : ViewModelBase
    {
        #region Members
        private readonly IIdentityService _identityService;
        private readonly ISessionService _sessionService;
        private string _userName;
        private string _password;
        #endregion
        #region Properties
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                Validate();
                OnPropertyChanged(nameof(UserName));
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                Validate();
                OnPropertyChanged(nameof(Password));
            }
        }
        #endregion
        #region Contructor
        public LoginVm()
        {
            _identityService = ServiceLocator.Current.GetInstance<IIdentityService>();
            _sessionService = ServiceLocator.Current.GetInstance<ISessionService>();
        }
        #endregion
        #region Public Methods
        public async Task<User> LoginAsync()
        {
            IsBusy = true;
            var user = await _identityService.LoginAsync(_userName, _password);
            if (user == null)
            {
                IsBusy = false;
                ValidateProperty(() => true, Constants.Messages.LoginError);
                return null;
            }

            if (!user.SessionActive)
            {
                _sessionService.AddToSession("logged", "true");
                if (user.UserType == Enumerations.UserType.Technician)
                {
                    _sessionService.AddToSession("technician", "true");
                }
            }
            else
            {
                ValidateProperty(() => true, Constants.Messages.HasLoggedSomewhereElse);
            }
            
            IsBusy = false;
            return user;
        }
        #endregion
        #region Private Methods
        protected override void Validate()
        {
            ValidateProperty(() => false, Constants.Messages.LoginError);
            ValidateProperty(() => false, Constants.Messages.HasLoggedSomewhereElse);
            ValidateProperty(() => string.IsNullOrEmpty(_userName) || string.IsNullOrEmpty(_password), Constants.Messages.ObligatoryField);
            ValidateProperty(() => !string.IsNullOrEmpty(_password) && _password.Length < 8, Constants.Messages.PasswordLenghtError);
            ValidateProperty(
               () =>
                   !Regex.IsMatch(_userName ?? string.Empty,
                       @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)",
                       RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)), Constants.Messages.WrongAccountFormat);

            base.Validate();
        }
        #endregion
    }
}