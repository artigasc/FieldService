using System;
using System.Linq;
using CoreGraphics;
using FESA.SCM.Core.Helpers;
using FESA.SCM.Core.Models;
using FESA.SCM.Core.ViewModels;
using FESA.SCM.iPhone.Controllers;
using FESA.SCM.iPhone.Helpers;
using Foundation;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using UIKit;

namespace FESA.SCM.iPhone.Controls
{
    public class AssignmentTableSource : UITableViewSource
    {
        private readonly AssignmentVm _assignmentVm;
        private readonly INavigationService _navigationService;

        public AssignmentTableSource()
        {
            _assignmentVm = ServiceLocator.Current.GetInstance<AssignmentVm>();
            _navigationService = ServiceLocator.Current.GetInstance<INavigationService>();

        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var assignment = _assignmentVm.Assignments[indexPath.Row];
            var cell = tableView.DequeueReusableCell("AssignmentCell") as AssignmentCell ?? new AssignmentCell();
            cell.SetAssignment(assignment, indexPath);
            cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
            return cell;
        }

        
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _assignmentVm.Assignments?.Count ?? 0;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            _assignmentVm.SelectedAssignment = _assignmentVm.Assignments[indexPath.Row];
            tableView.DeselectRow(indexPath, true);
            _navigationService.NavigateTo(Constants.PageKeys.Assignment);
        }

        public override void AccessoryButtonTapped(UITableView tableView, NSIndexPath indexPath)
        {
            _assignmentVm.SelectedAssignment = _assignmentVm.Assignments[indexPath.Row];
            tableView.DeselectRow(indexPath, true);
            _navigationService.NavigateTo(Constants.PageKeys.Assignment);
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            if (_assignmentVm.Assignments != null && _assignmentVm.Assignments.Any())
            {
                tableView.BackgroundView = null;
                tableView.SeparatorStyle = UITableViewCellSeparatorStyle.SingleLine;
                return 1;
            }

            var message = new UILabel
            {
                Frame = new CGRect(0,0,tableView.Bounds.Size.Width, tableView.Bounds.Size.Height),
                Text = "No hay data disponible, por favor jale para refrescar.",
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