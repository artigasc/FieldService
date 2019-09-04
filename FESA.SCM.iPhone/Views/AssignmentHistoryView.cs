using System;
using CoreGraphics;
using FESA.SCM.Core.ViewModels;
using FESA.SCM.iPhone.Controls;
using FESA.SCM.iPhone.Helpers;
using Foundation;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using UIKit;

namespace FESA.SCM.iPhone.Views
{
    public class AssignmentHistoryView : UIView
    {
        private readonly INavigationService _navigationService;
        private readonly AssignmentVm _assignmentVm;
        private readonly CGRect _bounds;
        private UITableView _assignmentsList;
        public AssignmentHistoryView(CGRect bounds)
        {
            _navigationService = ServiceLocator.Current.GetInstance<INavigationService>();
            _assignmentVm = ServiceLocator.Current.GetInstance<AssignmentVm>();
            _bounds = bounds;
            BuildInterface();
        }

        private void BuildInterface()
        {
            Frame = new CGRect(0, 0, _bounds.Width, _bounds.Height);
            _assignmentsList = new UITableView
            {
                Frame = new CGRect(0, 10, _bounds.Width, _bounds.Height - 10),
                BackgroundColor = UIColor.Clear,
                SeparatorColor = UIColor.LightGray,
                Source = new AssignmentHistoryTableSource(),
                RowHeight = 60
            };
            var refresh = new UIRefreshControl { TintColor = UIColor.Black };
            refresh.ValueChanged += (sender, args) =>
            {
                var attributedTitle = new NSAttributedString($"Última actualización: {DateTime.Today:MMM d, h:mm a}");
                var refreshControl = (UIRefreshControl)sender;
                refreshControl.AttributedTitle = attributedTitle;
                ReloadAssignments();
                refreshControl.EndRefreshing();
            };

            _assignmentsList.AddSubviews(refresh);
            AddSubviews(_assignmentsList);
            ReloadAssignments();
        }

        public void ReloadAssignments()
        {
            _assignmentVm.LoadHistoryAsync().ContinueWith(_ => {
                BeginInvokeOnMainThread(() => {
                    _assignmentsList.ReloadData();
                });
            });
        }
    }
}