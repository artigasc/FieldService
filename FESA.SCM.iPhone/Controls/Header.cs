using System;
using CoreGraphics;
using UIKit;

namespace FESA.SCM.iPhone.Controls
{
    public class Header : UIView
    {
        private readonly nfloat _width;
        private UIButton _button;
        private UILabel _label;

        public string LocationTitle
        {
            set
            {
                _label.Text = value;
            }
        }

        public EventHandler ButtonTouch
        {
            set { _button.TouchDown += value; }
        }

        public string LeftButtonImage
        {
            set
            {
                _button.SetImage(new UIImage(value), UIControlState.Normal);
            }
        }

        public Header(nfloat width)
        {
            _width = width;
            BuildInterface();
        }

        private void BuildInterface()
        {
            Frame = new CGRect(0, 0, _width, 70);
            BackgroundColor = UIColor.FromRGB(46, 63, 88);
            _label = new UILabel
            {
                Frame = new CGRect((_width / 2) - 100, 30, 200, 30),
                TextColor = UIColor.White,
                TextAlignment = UITextAlignment.Center,
                BackgroundColor = UIColor.Clear
            };
            _button = new UIButton
            {
                Frame = new CGRect(20, 30, 35, 35),
                BackgroundColor = UIColor.Clear,
                ImageEdgeInsets = new UIEdgeInsets(30, 25, 30, 25),
                ImageView =
                {
                    ContentMode = UIViewContentMode.ScaleAspectFit
                }
            };

            AddSubviews(_button, _label);
        }

        public void PerformOuterTouch()
        {
            _button.SendActionForControlEvents(UIControlEvent.TouchDown);
        }
    }
}