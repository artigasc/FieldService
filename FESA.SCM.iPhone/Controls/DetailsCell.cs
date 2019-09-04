using System.Collections.Generic;
using CoreGraphics;
using UIKit;

namespace FESA.SCM.iPhone.Controls
{
    public class DetailsCell : UITableViewCell
    {
        public void SetDetail(KeyValuePair<string, string> detail)
        {
            AutosizesSubviews = true;
            var name = new UILabel
            {
                Frame = new CGRect(30, 10, 120, 22),
                Text = detail.Key,
                Font = UIFont.FromName("Helvetica-Bold", 13),
                TextAlignment = UITextAlignment.Left,
                LineBreakMode = UILineBreakMode.TailTruncation,
                Lines = 0,
                AdjustsFontSizeToFitWidth = true
            };
            var value = new UILabel
            {
                Frame = new CGRect(name.Frame.Y + name.Frame.Width, 10, 160, 22),
                Text = detail.Value,
                Font = UIFont.FromName("Helvetica", 13),
                TextAlignment = UITextAlignment.Right,
                LineBreakMode = UILineBreakMode.TailTruncation,
                Lines = 0,
                AdjustsFontSizeToFitWidth = true
            };

            AddSubviews(name, value);
        }

    }
}