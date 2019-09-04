using CoreAnimation;
using CoreGraphics;
using UIKit;

namespace FESA.SCM.iPhone.Controls
{
    public class ActivityDate : UIView
    {
        private UILabel _titleLabel;
        private UILabel _dateLabel;
        public string Title
        {
            set { _titleLabel.Text = value; }
        }

        public string Date
        {
            set { _dateLabel.Text = value; }
        }
        public ActivityDate()
        {
            BuildInterface();
        }

        public void BuildInterface()
        {
            BackgroundColor = UIColor.Clear;
            Layer.MasksToBounds = true;
            Layer.AddSublayer(new CALayer
            {
                Frame =
                    new CGRect(0, Frame.Size.Height - 2.0f, Frame.Size.Width,
                        Frame.Size.Width),
                BackgroundColor = UIColor.White.CGColor,
                Opacity = 0.6f,
                BorderWidth = 1.0f,
                BorderColor = UIColor.White.CGColor
            });
            _titleLabel = new UILabel
            {
                Frame = new CGRect(0, 10, 120, 22),
                Font = UIFont.FromName("Helvetica-Light", 13),
                TextColor = UIColor.White,
            };
            _dateLabel = new UILabel
            {
                Frame = new CGRect(_titleLabel.Frame.X + _titleLabel.Frame.Width + 10, 10, 160, 22),
                Font = UIFont.FromName("Helvetica-Bold", 13),
                TextColor = UIColor.White,
            };
            AddSubviews(_titleLabel, _dateLabel);
        }
    }
}