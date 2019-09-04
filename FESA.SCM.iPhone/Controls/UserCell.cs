using CoreGraphics;
using FESA.SCM.Core.Models;
using UIKit;

namespace FESA.SCM.iPhone.Controls
{
    public class UserCell : UITableViewCell
    {
        public void SetUserData(User user)
        {
            AutosizesSubviews = true;
            var name = new UILabel
            {
                Frame = new CGRect(30, 10, 180, 44),
                Text = user.Name,
                Font = UIFont.FromName("Helvetica-Bold", 15),
                TextAlignment = UITextAlignment.Left,
                LineBreakMode = UILineBreakMode.TailTruncation,
                Lines = 0,
                AdjustsFontSizeToFitWidth = true
            };
            AddSubviews(name);
        }
    }
}