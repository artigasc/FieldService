using System;
using CoreAnimation;
using CoreGraphics;
using UIKit;

namespace FESA.SCM.iPhone.Controls
{
    public class ActivityFill : UIView
    {
        private UILabel _titleLabel;
        private UIButton _button;
        public string Title
        {
            set { _titleLabel.Text = value; }
            get { return _titleLabel.Text; }
        }

        public EventHandler Hit
        {
            set { _button.TouchUpInside += value; }
        }

        public string NewTitle
        {
            set
            {
                _button.SetTitle(value, UIControlState.Normal);
            }
        }

        public ActivityFill()
        {
            BuildInterface();
        }

        private void BuildInterface()
        {
            Layer.MasksToBounds = true;
            Layer.AddSublayer(new CALayer
            {
                Frame =
                    new CGRect(0, Frame.Size.Height - 2.0f, Frame.Size.Width,
                        Frame.Size.Width),
                BackgroundColor = UIColor.White.CGColor,
                Opacity = 0.6f,
                BorderWidth = 1.0f
            });
            _titleLabel = new UILabel
            {
                Frame = new CGRect(0, 10, 120, 22),
                Font = UIFont.FromName("Helvetica-Light", 13),
                TextColor = UIColor.White,
                Layer = { BorderColor = UIColor.White.CGColor }
            };
            _button = UIButton.FromType(UIButtonType.RoundedRect);
            _button.Frame = new CGRect(_titleLabel.Frame.X + _titleLabel.Frame.Width, 10, 160, 22);
            _button.TintColor = UIColor.White;
            _button.SetTitle("+", UIControlState.Normal);
            _button.Font = UIFont.FromName("Helvetica-Bold", 13);
            _button.TitleLabel.TextAlignment = UITextAlignment.Center;
            AddSubviews(_titleLabel, _button);
        }
    }
}