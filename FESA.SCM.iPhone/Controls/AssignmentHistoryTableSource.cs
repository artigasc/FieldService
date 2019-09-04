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
    public class AssignmentHistoryTableSource : UITableViewSource
    {
        private readonly AssignmentVm _assignmentVm;
        private readonly INavigationService _navigationService;

        public AssignmentHistoryTableSource()
        {
            _assignmentVm = ServiceLocator.Current.GetInstance<AssignmentVm>();
            _navigationService = ServiceLocator.Current.GetInstance<INavigationService>();
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var assignment = _assignmentVm.AssignmentHsitory[indexPath.Row];
            var cell = tableView.DequeueReusableCell("AssignmentCell") as AssignmentHistoryCell ?? new AssignmentHistoryCell();
            cell.SetAssignment(assignment, indexPath);
            cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
            return cell;
        }


        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _assignmentVm.AssignmentHsitory?.Count ?? 0;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            _assignmentVm.SelectedAssignment = _assignmentVm.AssignmentHsitory[indexPath.Row];
            tableView.DeselectRow(indexPath, true);
            _navigationService.NavigateTo(Constants.PageKeys.Assignment);
        }

        public override void AccessoryButtonTapped(UITableView tableView, NSIndexPath indexPath)
        {
            _assignmentVm.SelectedAssignment = _assignmentVm.AssignmentHsitory[indexPath.Row];
            tableView.DeselectRow(indexPath, true);
            _navigationService.NavigateTo(Constants.PageKeys.Assignment);
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            if (_assignmentVm.AssignmentHsitory != null && _assignmentVm.AssignmentHsitory.Any())
            {
                tableView.BackgroundView = null;
                tableView.SeparatorStyle = UITableViewCellSeparatorStyle.SingleLine;
                return 1;
            }

            var message = new UILabel
            {
                Frame = new CGRect(0, 0, tableView.Bounds.Size.Width, tableView.Bounds.Size.Height),
                Text = "No hay tiene Asignaciones historicas.",
                TextColor = UIColor.Black,
                Lines = 0,
                TextAlignment = UITextAlignment.Center,
                Font = UIFont.FromName("Helvetica-Bold", 15),
                AdjustsFontSizeToFitWidth = true
            };
            tableView.BackgroundView = message;
            tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            return 0;
        }
    }
}