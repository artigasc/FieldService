using System;
using CoreGraphics;
using FESA.SCM.Core.Models;
using FESA.SCM.Core.ViewModels;
using FESA.SCM.iPhone.Controls;
using FESA.SCM.iPhone.Helpers;
using Foundation;
using Microsoft.Practices.ServiceLocation;
using UIKit;

namespace FESA.SCM.iPhone.Views
{
    public class AssignmentsView : UIView
    {
        private readonly AssignmentVm _assignmentVm;
        private readonly CGRect _bounds;
        private UITableView _assignmentsList;

        public AssignmentsView(CGRect bounds)
        {
            _assignmentVm = ServiceLocator.Current.GetInstance<AssignmentVm>();
            _bounds = bounds;
            BuildInterface();
        }

        

        private async void BuildInterface()
        {
            await _assignmentVm.LoadAssignmentsAsync();
            Frame = new CGRect(0, 0, _bounds.Width, _bounds.Height);
            _assignmentsList = new UITableView
            {
                Frame = new CGRect(0, 10, _bounds.Width, _bounds.Height - 10),
                BackgroundColor = UIColor.Clear,
                SeparatorColor = UIColor.LightGray,
                Source = new AssignmentTableSource(),
                RowHeight = 60
            };
            var refresh = new UIRefreshControl {TintColor = UIColor.Black};
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
        }

        public async void ReloadAssignments()
        {
            var networStatus = Reachability.InternetConnectionStatus();
            if(networStatus != NetworkStatus.NotReachable)
                await _assignmentVm.SyncAssignments();
            else
                new UIAlertView(title: "Ferreyros", message: Constants.Messages.NoNetwork, del: null,
                            cancelButtonTitle: "OK", otherButtons: null).Show();
            BeginInvokeOnMainThread(() =>
            {
                _assignmentsList.ReloadData();
            });
        }
        

    }
}