using System.Threading.Tasks;
using FESA.SCM.Core.Helpers;
using FESA.SCM.Core.Models;
using FESA.SCM.Core.Services.Interfaces;
using Microsoft.Practices.ServiceLocation;

namespace FESA.SCM.Core.ViewModels
{
    public class ChangePasswordVm : ViewModelBase
    {
        #region Members
        private readonly IIdentityService _identityService;
        private string _password;
        private string _passwordConfirm;
        #endregion
        #region Properties
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

        public string PasswordConfirm
        {
            get { return _passwordConfirm; }
            set
            {
                _passwordConfirm = value;
                Validate();
                OnPropertyChanged(nameof(PasswordConfirm));
            }
        }
        #endregion
        #region Constructor
        public ChangePasswordVm()
        {
            _identityService = ServiceLocator.Current.GetInstance<IIdentityService>();
        }
        #endregion
        #region Public Methods
        public async Task<bool> ChangePasswordAsync()
        {
            var user = await Database.Connection.Table<User>().FirstAsync();
            IsBusy = true;
            var changed = await _identityService.ChangePasswordAsync(user.Id, _password);
            IsBusy = false;
            return changed;
        }
        #endregion
        #region Private Methods
        protected override void Validate()
        {
            ValidateProperty(() => string.IsNullOrEmpty(_password) || string.IsNullOrEmpty(_passwordConfirm), Constants.Messages.ObligatoryField);
            ValidateProperty(() => !string.IsNullOrEmpty(_password) && _password.Length < 8, Constants.Messages.PasswordLenghtError);
            ValidateProperty(() => _password != _passwordConfirm, Constants.Messages.PasswordError);
            base.Validate();
        }
        #endregion
    }
}