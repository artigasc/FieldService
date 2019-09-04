using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FESA.SCM.Android.Activities;
using FESA.SCM.Android.Helpers;
using FESA.SCM.Core.Helpers;
using FESA.SCM.Core.Models;
using FESA.SCM.Core.Services.Implementation;
using FESA.SCM.Core.Services.Interfaces;
using FESA.SCM.Core.ViewModels;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;

namespace FESA.SCM.Android
{
    [Application(Label = "Servicio de Campo")]
    public class Application : global::Android.App.Application
    {
        public Application(IntPtr javaReference, JniHandleOwnership transfer)
            :base(javaReference, transfer)
        { }

        public override void OnCreate()
        {
            base.OnCreate();
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<IIdentityService>(() => new IdentityService());
            SimpleIoc.Default.Register<IAssignmentService>(() => new AssignmentService());
            SimpleIoc.Default.Register<ISqlite>(() => new Sqlite());
            SimpleIoc.Default.Register<ISessionService>(() => new SessionService());
            SimpleIoc.Default.Register<LoginVm>();
            SimpleIoc.Default.Register<ChangePasswordVm>();
            SimpleIoc.Default.Register<RestorePasswordVm>();
            SimpleIoc.Default.Register<AssignmentVm>();
            SimpleIoc.Default.Register<MenuVm>();
            SimpleIoc.Default.Register<INavigationService>(ConfigureNavigation);
        }

        private NavigationService ConfigureNavigation()
        {
            var nav = new NavigationService();

            nav.Configure(Constants.PageKeys.Login, typeof(LoginActivity));
            //nav.Configure(Constants.PageKeys.ChangePassword, typeof(ChangePasswordViewController));
            //nav.Configure(Constants.PageKeys.RestorePassword, typeof(RestorePasswordViewController));
            //nav.Configure(Constants.PageKeys.Home, typeof(HomeViewController));
            //nav.Configure(Constants.PageKeys.Assignment, typeof(AssignmentViewController));
            //nav.Configure(Constants.PageKeys.ServiceData, typeof(ServiceDataViewController));
            //nav.Configure(Constants.PageKeys.Documents, typeof(DocumentsViewController));
            //nav.Configure(Constants.PageKeys.ActivityStart, typeof(ActivityStartViewController));
            return nav;
        }
    }
}