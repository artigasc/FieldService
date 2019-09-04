using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FESA.SCM.Core.Helpers;
using FESA.SCM.Core.Models;
using FESA.SCM.Core.Services.Interfaces;
using Microsoft.Practices.ServiceLocation;

namespace FESA.SCM.Core.ViewModels
{
    public class RestorePasswordVm : ViewModelBase
    {
        #region Members
        private readonly IIdentityService _identityService;
        private string _userName;
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
        #endregion
        #region Constructor
        public RestorePasswordVm()
        {
            _identityService = ServiceLocator.Current.GetInstance<IIdentityService>();
        }
        #endregion
        #region Public Methods
        public async Task<bool> RestorePasswordAsync()
        {
            IsBusy = true;
            var changed = await _identityService.ResetPasswordAsync(_userName);
            IsBusy = false;
            return changed;
        }
        #endregion
        #region Private Methods
        protected override void Validate()
        {
            ValidateProperty(() => string.IsNullOrEmpty(_userName), Constants.Messages.ObligatoryField);
            ValidateProperty(
                () =>
                    !Regex.IsMatch(_userName ?? string.Empty,
                        @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?",
                        RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)), Constants.Messages.ObligatoryField);

            base.Validate();
        }
        #endregion
    }
}