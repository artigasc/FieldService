using System;
using FESA.SCM.Core.Models;
using FESA.SCM.Core.ViewModels;
using FESA.SCM.iPhone.Helpers;
using Foundation;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using UIKit;

namespace FESA.SCM.iPhone.Controls
{
    public class UserTableSource : UITableViewSource
    {
        private readonly ReportVm _reportVm;
        private readonly INavigationService _navigationService;
        public UserTableSource()
        {
            _reportVm = ServiceLocator.Current.GetInstance<ReportVm>();
            _navigationService = ServiceLocator.Current.GetInstance<INavigationService>();
        }
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var user = _reportVm.Users[indexPath.Row];
            var cell = tableView.DequeueReusableCell("UserCell") as UserCell ?? new UserCell();
            cell.SetUserData(user);
            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _reportVm.Users.Count;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            _reportVm.SelectedUser = _reportVm.Users[indexPath.Row];
            _navigationService.NavigateTo(Constants.PageKeys.UsersTimeLine);
        }
    }
}