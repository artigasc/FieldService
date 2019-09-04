using System;
using CoreGraphics;
using UIKit;

namespace FESA.SCM.iPhone.Controls
{
    public class TextImageButton : UIButton
    {
        public string Content
        {
            set
            {
                SetTitle(value, UIControlState.Normal);
            }
        }

        public string Image
        {
            set
            {
                SetBackgroundImage(new UIImage(value), UIControlState.Normal);
            }
        }
        public TextImageButton():base(UIButtonType.Custom)
        {
            BuildInterface();
        }

        private void BuildInterface()
        {
            BackgroundColor = UIColor.Clear;
            TitleLabel.Font = UIFont.FromName("Helvetica-Light", 8f);
            TitleLabel.TextAlignment = UITextAlignment.Center;
            TitleLabel.LineBreakMode = UILineBreakMode.WordWrap;
            ImageView.ContentMode = UIViewContentMode.Top;
            TitleEdgeInsets = new UIEdgeInsets(0, -40, -90, -40);
        }
    }
}