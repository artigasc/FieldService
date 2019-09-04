using System;
using System.Linq;
using CoreGraphics;
using FESA.SCM.Core.Models;
using FESA.SCM.Core.ViewModels;
using FESA.SCM.iPhone.Helpers;
using Foundation;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using UIKit;

namespace FESA.SCM.iPhone.Controls
{
    public class ActivityTableSource : UITableViewSource
    {
        private readonly AssignmentVm _assignmentVm;
        private readonly INavigationService _navigationService;
        public ActivityTableSource()
        {
            _assignmentVm = ServiceLocator.Current.GetInstance<AssignmentVm>();
            _navigationService = ServiceLocator.Current.GetInstance<INavigationService>();
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var activity = _assignmentVm.SelectedAssignment.Activities[indexPath.Row];
            var cell = tableView.DequeueReusableCell("ActivityCell") as ActivityCell ?? new ActivityCell();
            cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
            cell.SetActivity(activity);
            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _assignmentVm.SelectedAssignment.Activities.Count;
        }

        public override void AccessoryButtonTapped(UITableView tableView, NSIndexPath indexPath)
        {
            _assignmentVm.SelectedActivity = _assignmentVm.SelectedAssignment.Activities[indexPath.Row];
            tableView.DeselectRow(indexPath, true);
            _navigationService.NavigateTo(Constants.PageKeys.ActivityStart);
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            _assignmentVm.SelectedActivity = _assignmentVm.SelectedAssignment.Activities[indexPath.Row];
            tableView.DeselectRow(indexPath, true);
            _navigationService.NavigateTo(Constants.PageKeys.ActivityStart);
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            if (_assignmentVm.SelectedAssignment.Activities != null && _assignmentVm.SelectedAssignment.Activities.Any())
            {
                tableView.BackgroundView = null;
                tableView.SeparatorStyle = UITableViewCellSeparatorStyle.SingleLine;
                return 1;
            }

            var message = new UILabel
            {
                Frame = new CGRect(0, 0, tableView.Bounds.Size.Width, tableView.Bounds.Size.Height),
                Text = "No has realizado actividades, para agregar una, picale al boton verde.",
                TextColor = UIColor.Black,
                LineBreakMode = UILineBreakMode.WordWrap,
                Lines = 0,
                TextAlignment = UITextAlignment.Center,
                Font = UIFont.FromName("Helvetica-Bold", 12),
                AdjustsFontSizeToFitWidth = true
            };
            tableView.BackgroundView = message;
            tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            return 0;
        }
    }
}