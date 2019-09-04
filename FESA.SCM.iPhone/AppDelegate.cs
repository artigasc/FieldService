using System;
using System.Collections;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FESA.SCM.Core.Helpers;
using FESA.SCM.Core.Models;
using FESA.SCM.Core.Services.Implementation;
using FESA.SCM.Core.Services.Interfaces;
using FESA.SCM.Core.ViewModels;
using FESA.SCM.iPhone.Controllers;
using FESA.SCM.iPhone.Helpers;
using Foundation;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using WindowsAzure.Messaging;
//using ModernHttpClient;
using Newtonsoft.Json;
using UIKit;

namespace FESA.SCM.iPhone
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations

        public override UIWindow Window { get; set; }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            Window = new UIWindow(UIScreen.MainScreen.Bounds);

            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            DispatcherHelper.Initialize(application);
            SimpleIoc.Default.Register<IIdentityService>(() => new IdentityService());
            SimpleIoc.Default.Register<IAssignmentService>(() => new AssignmentService());
            SimpleIoc.Default.Register<IReportService>(()=> new ReportService());
            SimpleIoc.Default.Register<ISqlite>(() => new Sqlite());
            SimpleIoc.Default.Register<ISessionService>(() => new SessionService());
            SimpleIoc.Default.Register<INotificationService>(() => new NotificationService());
            SimpleIoc.Default.Register<LoginVm>();
            SimpleIoc.Default.Register<ChangePasswordVm>();
            SimpleIoc.Default.Register<RestorePasswordVm>();
            SimpleIoc.Default.Register<AssignmentVm>();
            SimpleIoc.Default.Register<MenuVm>();
            SimpleIoc.Default.Register<ReportVm>();

            var navigationService = ConfigureNavigation();
            SimpleIoc.Default.Register<INavigationService>(() => navigationService);

            UINavigationController navigationController = null;
            var session = ServiceLocator.Current.GetInstance<ISessionService>().GetFromSession("logged");
            navigationController = string.IsNullOrEmpty(session)
                ? new UINavigationController(new LoginViewController())
                : new UINavigationController(new HomeViewController());
            navigationService.Initialize(navigationController);
            Window.RootViewController = navigationController;

            Window.MakeKeyAndVisible();

            return true;
        }

        private FesaNavigationService ConfigureNavigation()
        {
            var nav = new FesaNavigationService();
            
            nav.Configure(Constants.PageKeys.Login, typeof(LoginViewController));
            nav.Configure(Constants.PageKeys.ChangePassword, typeof(ChangePasswordViewController));
            nav.Configure(Constants.PageKeys.RestorePassword, typeof(RestorePasswordViewController));
            nav.Configure(Constants.PageKeys.Home, typeof(HomeViewController));
            nav.Configure(Constants.PageKeys.Assignment, typeof(AssignmentViewController));
            nav.Configure(Constants.PageKeys.ServiceData, typeof(ServiceDataViewController));
            nav.Configure(Constants.PageKeys.Documents, typeof(DocumentsViewController));
            nav.Configure(Constants.PageKeys.ActivityStart, typeof(ActivityStartViewController));
            nav.Configure(Constants.PageKeys.UsersPerState, typeof(UsersPerStateViewController));
            nav.Configure(Constants.PageKeys.UsersTimeLine, typeof(UserAssignmentTimeLineViewController));
            nav.Configure(Constants.PageKeys.AssignmentsPerClient, typeof(AssignmentsPerClientViewController));
            return nav;
        }

        public override void OnResignActivation(UIApplication application)
        {
            // Invoked when the application is about to move from active to inactive state.
            // This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
            // or when the user quits the application and it begins the transition to the background state.
            // Games should use this method to pause the game.
        }

        public override void DidEnterBackground(UIApplication application)
        {
            // Use this method to release shared resources, save user data, invalidate timers and store the application state.
            // If your application supports background exection this method is called instead of WillTerminate when the user quits.
            var assignmentVm = ServiceLocator.Current.GetInstance<AssignmentVm>();
            assignmentVm.StopAsync(willContinue: assignmentVm.ActiveActivity != null).GetAwaiter().GetResult();
        }

        public override void WillEnterForeground(UIApplication application)
        {
            // Called as part of the transiton from background to active state.
            // Here you can undo many of the changes made on entering the background.
        }

        public override void OnActivated(UIApplication application)
        {
            UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
        }

        public override void WillTerminate(UIApplication application)
        {
            // Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
            var assignmentVm = ServiceLocator.Current.GetInstance<AssignmentVm>();
            assignmentVm.StopAsync(willContinue: assignmentVm.ActiveActivity != null).GetAwaiter().GetResult();
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            RetrieveOrRequestRegistrationIdWithDeviceToken(deviceToken.Description, (s, e) =>
            {
                if (e != null)
                {
                    Console.WriteLine(e.Description);
                    return;
                }

                var tags = new[] {"SCM"};
                UpsertRegistrationWithRegistrationId(s, deviceToken.Description, tags, (err) =>
                {
                    if (err != null)
                        Console.WriteLine("Error: " + err.Description);
                    else
                        Console.WriteLine("Success");
                });
            });

        }

        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            ProcessNotification(userInfo, false);
        }

        private async void RetrieveOrRequestRegistrationIdWithDeviceToken(string token, Action<string, NSError> callback)
        {
            var sessionService = ServiceLocator.Current.GetInstance<ISessionService>();
            var registrationId = sessionService.GetFromSession("registrationId");
            if (!string.IsNullOrEmpty(registrationId))
            {
                callback(registrationId, null);
                return;
            }
            var notificationRegister = ServiceLocator.Current.GetInstance<INotificationService>();
            registrationId = await notificationRegister.PostToNotificationService(token);
            if (!string.IsNullOrEmpty(registrationId))
            {
                callback(registrationId, null);
            }
            else
            {
                callback(null, new NSError(domain: (NSString)"Registration", code: 400));
            }
        }

        private async void UpsertRegistrationWithRegistrationId(string registrationId, string token, string[] tags,
            Action<NSError> callback)
        {
            var user = await ServiceLocator.Current.GetInstance<IIdentityService>().GetUserDataAsync();
            var deviceRegistration = new
            {
                Platform = "apns",
                Handle = token.Trim('<', '>').Replace(" ", ""),
                Tags = tags,
                UserId = user.Id
            };
            registrationId = JsonConvert.DeserializeObject<string>(registrationId);
            var drSerialized = JsonConvert.SerializeObject(deviceRegistration);
            var notificationRegister = ServiceLocator.Current.GetInstance<INotificationService>();
            var success = await notificationRegister.PutToNotificationService(registrationId, drSerialized);
            callback(success ? null : new NSError(domain: (NSString) "Update", code: 400));
        }

        private void ProcessNotification(NSDictionary options, bool fromFinishedLaunching)
        {
            if (null == options || !options.ContainsKey(new NSString("aps"))) return;
            var aps = options.ObjectForKey(new NSString("aps")) as NSDictionary;

            var alert = string.Empty;

            if (aps != null && aps.ContainsKey(new NSString("alert")))
                alert = (aps[new NSString("alert")] as NSString)?.ToString();

            if (fromFinishedLaunching) return;
            if (string.IsNullOrEmpty(alert)) return;
            var avAlert = new UIAlertView("Ferreyros", alert, null, "OK", null);
            avAlert.Show();
        }
    }
}


