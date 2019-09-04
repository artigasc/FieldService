using System;
using System.Collections.Generic;
using CoreGraphics;
using FESA.SCM.iPhone.Controllers;
using FESA.SCM.iPhone.Views;
using Foundation;
using UIKit;

namespace FESA.SCM.iPhone.Controls
{
    public class MenuTableSource : UITableViewSource
    {
        private readonly List<KeyValuePair<string,int>> _options;

        public MenuTableSource(List<KeyValuePair<string, int>> options)
        {
            if(options == null)
                throw new ArgumentNullException(nameof(options));

            _options = options;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell("OptionsCell") as MenuOptionCell ?? new MenuOptionCell("OptionsCell");

            var option = _options[indexPath.Row];

            cell.TextLabel.Text = option.Key;
            if(option.Value >= 0)
                cell.DetailTextLabel.Text = option.Value.ToString();

            return cell;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            HomeViewController.HomeLayout.SelectContent(indexPath.Row);
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _options.Count;
        }
    }
}