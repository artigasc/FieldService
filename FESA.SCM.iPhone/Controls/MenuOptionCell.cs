using CoreGraphics;
using UIKit;

namespace FESA.SCM.iPhone.Controls
{
    public class MenuOptionCell : UITableViewCell
    {
        public MenuOptionCell(string cellId):base(UITableViewCellStyle.Value1, cellId)
        {
            BuildInterface();
        }

        private void BuildInterface()
        {
            BackgroundColor = UIColor.Clear;
        }

        public override UIView SelectedBackgroundView { get; set; }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            DetailTextLabel.Frame = new CGRect(DetailTextLabel.Frame.X - 40, DetailTextLabel.Frame.Y - 5,
                DetailTextLabel.Frame.Width + 20, DetailTextLabel.Frame.Height + 10);
            
            SelectedBackgroundView = new UIView(Frame)
            {
                BackgroundColor = UIColor.White.ColorWithAlpha(0.3f)
            };
            DetailTextLabel.Layer.CornerRadius = 10;
            DetailTextLabel.ClipsToBounds = true;
            TextLabel.TextColor = UIColor.White;
            TextLabel.TextAlignment = UITextAlignment.Center;
            DetailTextLabel.TextColor = UIColor.White;
            DetailTextLabel.TextAlignment = UITextAlignment.Center;
            DetailTextLabel.Font = UIFont.FromName("Helvetica-Bold", 10f);
            DetailTextLabel.BackgroundColor = TextLabel.Text.ToLower().Contains("historico")
                ? UIColor.Black
                : UIColor.FromRGB(249, 195, 22);
        }
    }
}