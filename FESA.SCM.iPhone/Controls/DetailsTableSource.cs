using System;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using Foundation;
using UIKit;

namespace FESA.SCM.iPhone.Controls
{
    public class DetailsTableSource : UITableViewSource
    {
        private readonly List<KeyValuePair<string, string>> _datosDetalle;
        public DetailsTableSource(List<KeyValuePair<string, string>> datosDetalle)
        {
            if (datosDetalle == null)
                throw new ArgumentNullException(nameof(datosDetalle));

            _datosDetalle = datosDetalle;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var detail = _datosDetalle[indexPath.Row];
            var cell = tableView.DequeueReusableCell("DetailCell") as DetailsCell ?? new DetailsCell();
            cell.SetDetail(detail);
            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _datosDetalle.Count;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            if (_datosDetalle.Any())
            {
                tableView.BackgroundView = null;
                tableView.SeparatorStyle = UITableViewCellSeparatorStyle.SingleLine;
                return 1;
            }

            var message = new UILabel
            {
                Frame = new CGRect(0, 0, tableView.Bounds.Size.Width, tableView.Bounds.Size.Height),
                Text = "No existen datos para el criterio seleccionado.",
                TextColor = UIColor.White,
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