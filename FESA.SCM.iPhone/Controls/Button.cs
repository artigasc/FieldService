using System;
using System.ComponentModel;
using System.Linq;
using CoreGraphics;
using FESA.SCM.Core.Models;
using Foundation;
using UIKit;

namespace FESA.SCM.iPhone.Controls
{
    public class Button : UIButton
    {

        [Export("After"), Browsable(true)]
        public UIView After
        {
            set
            {
                Frame = new CGRect(20, value.Frame.Y + value.Frame.Height + Constants.TextboxMargin, value.Frame.Width - 40,
                    Constants.ButtonHeight);
            }
        }

        [Export("Content"), Browsable(true)]
        public string Content
        {
            set
            {
                SetTitle(value, UIControlState.Normal);
            }
        }

        public Button(UIButtonType type) : base(type)
        {
            Initialize();
        }

        public Button()
        {
            
            // Called when created from code.
            Initialize();
        }

        private void Initialize()
        {
            HorizontalAlignment = UIControlContentHorizontalAlignment.Center;
            VerticalAlignment = UIControlContentVerticalAlignment.Center;
            BackgroundColor = UIColor.Yellow;
            TitleLabel.TextAlignment = UITextAlignment.Center;
            Layer.CornerRadius = 9f;
            TintColor = UIColor.Black;
            TitleLabel.Font = UIFont.FromName("Helvetica-Bold", 20f);
            TitleColor(UIControlState.Normal);
        }
    }
}