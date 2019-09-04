using System;
using Foundation;
using UIKit;

namespace FESA.SCM.iPhone.Helpers
{
    public static class UiKitExtensions
    {
        public static void SetDidChangeNotification(this UITextField textField, Action<UITextField> callback)
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            NSNotificationCenter.DefaultCenter.AddObserver(UITextField.TextFieldTextDidChangeNotification, _ => callback(textField), textField);
        }
    }
}