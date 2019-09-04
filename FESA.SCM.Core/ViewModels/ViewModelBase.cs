using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FESA.SCM.Core.ViewModels
{
    public class ViewModelBase : PropertyChangedBase
    {
        public event EventHandler IsBusyChanged;
        public event EventHandler IsValidChanged;

        private bool _isBusy;

        public bool IsValid => Errors.Count == 0;

        protected List<string> Errors { get; } = new List<string>();

        public virtual string Error
            => Errors.Count > 0 ? Errors.Last() : string.Empty;

        protected virtual void Validate()
        {
            OnPropertyChanged(nameof(IsValid));
            OnPropertyChanged(nameof(Errors));

            IsValidChanged?.Invoke(this, EventArgs.Empty);
        }
    
        protected virtual void ValidateProperty(Func<bool> validate, string error)
        {
            if (validate())
            {
                if (!Errors.Contains(error))
                    Errors.Add(error);
            }
            else
            {
                Errors.Remove(error);
            }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy == value) return;
                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
                OnIsBusyChanged();
            }
        }

        protected virtual void OnIsBusyChanged()
        {
            IsBusyChanged?.Invoke(this, EventArgs.Empty);
        }

    }
}