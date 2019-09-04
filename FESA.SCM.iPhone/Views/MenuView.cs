using System;
using System.Collections.Generic;
using CoreGraphics;
using FESA.SCM.Core.Services.Interfaces;
using FESA.SCM.Core.ViewModels;
using FESA.SCM.iPhone.Controls;
using FESA.SCM.iPhone.Helpers;
using Microsoft.Practices.ServiceLocation;
using UIKit;

namespace FESA.SCM.iPhone.Views
{
    public class MenuView : UIView
    {
        private readonly UIView _outerView;
        private readonly MenuVm _menuVm;
        private readonly nfloat _navWidth;
        private readonly nfloat _navHeight;

        public MenuView(UIView outerView, nfloat navWidth, nfloat navHeight)
        {
            _menuVm = ServiceLocator.Current.GetInstance<MenuVm>();
            _outerView = outerView;
            _navHeight = navHeight;
            _navWidth = navWidth;
            BuildInterface();
        }

        private async void BuildInterface()
        {
            BackgroundColor = new UIImage("Images/fondoPrincipal.jpg").GetScaledImageBackground(_outerView);
            Frame = new CGRect(0, 0, _navWidth, _navHeight);

            await _menuVm.LoadData();

            var userImage = new UIImageView
            {
                Image = await _menuVm.User.Photo.GetImageFromUrl(),
                Frame = new CGRect((_navWidth / 4) + 10, 50, 100, 100),
                ClipsToBounds = true,
                Layer =
                {
                    BorderWidth = 3f,
                    BorderColor = UIColor.White.CGColor,
                    CornerRadius = 50
                }
            };

            var userName = new UILabel
            {
                Frame = new CGRect(10, userImage.Frame.Y + userImage.Frame.Height + 10, _navWidth - 20, 60),
                LineBreakMode = UILineBreakMode.WordWrap,
                TextAlignment = UITextAlignment.Center,
                Lines = 2,
                Text = _menuVm.User.Name,
                TextColor = UIColor.White,
                Font = UIFont.FromName("Helvetica-Bold", 16)
            };


            var userStatus = new UILabel
            {
                Frame = new CGRect(0, userName.Frame.Y + userName.Frame.Height + 10, _navWidth, 30),
                LineBreakMode = UILineBreakMode.WordWrap,
                TextAlignment = UITextAlignment.Center,
                Lines = 2,
                Text = Utils.GetUserStatusText(_menuVm.User.UserStatus),
                TextColor = UIColor.White,
                Font = UIFont.FromName("Helvetica-Light", 16)
            };

            var menu =
                new UITableView(new CGRect(0, userStatus.Frame.Y + userStatus.Frame.Height + 20, _navWidth,
                    _navHeight - userImage.Frame.Height - userName.Frame.Height - userStatus.Frame.Height))
                {
                    BackgroundColor = UIColor.Clear,
                    SeparatorColor = UIColor.Clear,
                    ScrollEnabled = false
                };

            var session = ServiceLocator.Current.GetInstance<ISessionService>().GetFromSession("technician");
            List<KeyValuePair<string, int>> menuItems;
            if (!string.IsNullOrEmpty(session))
            {
                menuItems = new List<KeyValuePair<string, int>>
                {
                    new KeyValuePair<string, int>("Servicios", _menuVm.AsignmentCount),
                    new KeyValuePair<string, int>("Historico de Servicios", _menuVm.HistoryAsignmentCount)
                };
            }
            else
            {
                menuItems = new List<KeyValuePair<string, int>>
                {
                    new KeyValuePair<string, int>("Ocupabilidad de Operarios", -1),
                    new KeyValuePair<string, int>("OT por Clientes", -1)
                };
            }
            menuItems.Add(new KeyValuePair<string, int>("Cerrar Sesión", -1));

            var tablesource = new MenuTableSource(menuItems);
            menu.Source = tablesource;

            AddSubviews(userImage, userName, userStatus, menu);

        }
        
    }
}