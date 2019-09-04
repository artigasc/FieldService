using System;
using Foundation;
using UIKit;

namespace FESA.SCM.iPhone.Controllers
{
    public class BaseController : UIViewController
    {
        public virtual bool HandlesKeyboardNotifications => false;

        
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            if (!HandlesKeyboardNotifications) return;
            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, OnKeyboardNotification);
            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, OnKeyboardNotification);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            UINavigationBar.Appearance.Translucent = true;
            NavigationController.SetNavigationBarHidden(true, true);
            NavigationController.SetToolbarHidden(true, true);
            UIApplication.SharedApplication.SetStatusBarHidden(true, true);
            View.Alpha = 0f;
            UIView.Animate(.3, 0f, UIViewAnimationOptions.CurveEaseInOut, () => View.Alpha = 1f, null);
        }
        
        public override bool ShouldAutorotate()
        {
            return false;
        }

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
        {
            return UIInterfaceOrientationMask.Portrait;
        }

        private void OnKeyboardNotification(NSNotification notification)
        {
            if (!IsViewLoaded)
                return;

            var visible = notification.Name == UIKeyboard.WillShowNotification;

            UIView.BeginAnimations("AnimateForKeyboard");
            UIView.SetAnimationBeginsFromCurrentState(true);
            UIView.SetAnimationDuration(UIKeyboard.AnimationDurationFromNotification(notification));
            UIView.SetAnimationCurve((UIViewAnimationCurve)UIKeyboard.AnimationCurveFromNotification(notification));

            var landscape = InterfaceOrientation == UIInterfaceOrientation.LandscapeLeft || InterfaceOrientation == UIInterfaceOrientation.LandscapeRight;
            if (visible)
            {
                var keyboardFrame = UIKeyboard.FrameEndFromNotification(notification);
                OnKeyboardChanged(visible, landscape ? keyboardFrame.Width : keyboardFrame.Height);
            }
            else
            {
                var keyboardFrame = UIKeyboard.FrameBeginFromNotification(notification);
                OnKeyboardChanged(visible, landscape ? keyboardFrame.Width : keyboardFrame.Height);
            }

            UIView.CommitAnimations();
        }

        protected virtual void OnKeyboardChanged(bool visible, nfloat height)
        {

        }
        
    }
}
