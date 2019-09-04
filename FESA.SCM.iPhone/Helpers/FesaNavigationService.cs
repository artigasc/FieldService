using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FESA.SCM.iPhone.Controllers;
using GalaSoft.MvvmLight.Views;
using UIKit;

namespace FESA.SCM.iPhone.Helpers
{
    public class FesaNavigationService : INavigationService
    {
        private readonly Dictionary<string, Type> _pagesByKey = new Dictionary<string, Type>();
        private UINavigationController _navigation;

        public string CurrentPageKey
        {
            get
            {
                lock (_pagesByKey)
                {
                    if (_navigation.VisibleViewController == null)
                    {
                        return null;
                    }
                    var pageType = _navigation.VisibleViewController.GetType();
                    return _pagesByKey.ContainsValue(pageType)
                        ? _pagesByKey.FirstOrDefault(x => x.Value == pageType).Key
                        : null;
                }
            }
        }

        public void GoBack()
        {
            _navigation.PopViewController(animated: true);
        }

        public void NavigateTo(string pageKey)
        {
            NavigateTo(pageKey, null);
        }

        public void NavigateTo(string pageKey, object parameter)
        {
            lock (_pagesByKey)
            {
                if (_pagesByKey.ContainsKey(pageKey))
                {
                    var type = _pagesByKey[pageKey];
                    ConstructorInfo constructor;
                    object[] parameters;
                    if (parameter == null)
                    {
                        constructor =
                            type.GetTypeInfo().DeclaredConstructors.FirstOrDefault(c => !c.GetParameters().Any());
                        parameters = new object[] { };
                    }
                    else
                    {
                        constructor = type.GetTypeInfo().DeclaredConstructors.FirstOrDefault(x =>
                        {
                            var p = x.GetParameters();
                            return p.Count() == 1 && p[0].ParameterType == parameter.GetType();
                        });
                        parameters = new[] { parameter };
                    }
                    if (constructor == null)
                    {
                        throw new InvalidOperationException("No suitable constructor found for page " + pageKey);
                    }
                    var page = constructor.Invoke(parameters) as UIViewController;
                    _navigation.PushViewController(page, animated: true);
                }
                else
                {
                    throw new ArgumentException(
                        $"No such page: {pageKey}. Did you forget to call NavigationService.Configure?");
                }
            }
        }

        public void Configure(string pageKey, Type pageType)
        {
            lock (_pagesByKey)
            {
                if (_pagesByKey.ContainsKey(pageKey))
                {
                    _pagesByKey[pageKey] = pageType;
                }
                else
                {
                    _pagesByKey.Add(pageKey, pageType);
                }
            }
        }

        public void Initialize(UINavigationController navigation)
        {
            _navigation = navigation;
        }
    }
}