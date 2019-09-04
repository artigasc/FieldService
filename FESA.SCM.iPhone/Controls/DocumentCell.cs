using CoreGraphics;
using FESA.SCM.Core.Models;
using UIKit;

namespace FESA.SCM.iPhone.Controls
{
    public class DocumentCell : UITableViewCell
    {
        public void SetDocument(Document document)
        {
            BackgroundColor = UIColor.Clear;
            var documentName = new UILabel
            {
                Frame = new CGRect(30, 10, 200, 40),
                Font = UIFont.FromName("Helvetica-Bold", 13),
                TextAlignment = UITextAlignment.Left,
                LineBreakMode = UILineBreakMode.TailTruncation,
                TextColor = UIColor.White,
                BackgroundColor = UIColor.Clear,
                Lines = 0,
                AdjustsFontSizeToFitWidth = true,
                Text = document.Name
            };
            var check = new UIImageView(new UIImage("Images/btn-check.png"))
            {
                Frame = new CGRect(documentName.Frame.X + documentName.Frame.Width + 5, 20, 20, 20),
                Hidden = string.IsNullOrEmpty(document.Id)
            };
            AddSubviews(documentName, check);
        }
        
    }
}