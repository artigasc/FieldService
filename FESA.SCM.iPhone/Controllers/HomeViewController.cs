using FESA.SCM.iPhone.Views;
using Foundation;
using Syncfusion.SfNavigationDrawer.iOS;
using UIKit;

namespace FESA.SCM.iPhone.Controllers
{
    public class HomeViewController : BaseController
    {
        private SFNavigationDrawer _navigationDrawer;
        public static HomeLayoutView HomeLayout;
        
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            BuildInterface();
        }

        private void BuildInterface()
        {
            HomeLayout = new HomeLayoutView(View.Frame);

            _navigationDrawer = new SFNavigationDrawer
            {
                DrawerHeight = View.Bounds.Height,
                DrawerWidth = (View.Bounds.Width*80)/100
            };

            HomeLayout.MenuButtonTouched = (sender, args) => { _navigationDrawer.ToggleDrawer(); };

            _navigationDrawer.DrawerContentView = new MenuView(View, _navigationDrawer.DrawerWidth, _navigationDrawer.DrawerHeight);

            _navigationDrawer.ContentView = HomeLayout;
            _navigationDrawer.Position = SFNavigationDrawerPosition.SFNavigationDrawerPositionLeft;
            _navigationDrawer.Transition = SFNavigationDrawerTransition.SFNavigationDrawerTransitionSlideOnTop;

            if (!UIApplication.SharedApplication.IsRegisteredForRemoteNotifications)
            {
                if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
                {
                    var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(
                           UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                           new NSSet());

                    UIApplication.SharedApplication.RegisterUserNotificationSettings(pushSettings);
                    UIApplication.SharedApplication.RegisterForRemoteNotifications();
                }
                else
                {
                    var notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge |
                                            UIRemoteNotificationType.Sound |
                                            UIRemoteNotificationType.NewsstandContentAvailability;
                    UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
                }
            }
            View.AddSubviews(_navigationDrawer.View);
        }
    }
}