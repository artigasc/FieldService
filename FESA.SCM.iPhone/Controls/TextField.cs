using System;
using System.ComponentModel;
using System.Linq;
using CoreAnimation;
using CoreGraphics;
using FESA.SCM.Core.Models;
using Foundation;
using UIKit;

namespace FESA.SCM.iPhone.Controls
{
    [Register("TextField"), DesignTimeVisible(true)]
    public class TextField : UITextField
    {
        [Export("After"), Browsable(true)]
        public UIView After
        {
            set
            {
                Frame = new CGRect(0, value.Frame.Y + value.Frame.Height + Constants.TextboxMargin, value.Frame.Width,
                    Constants.TextboxHeight);
                var borderLayer = Layer.Sublayers.FirstOrDefault();
                if (borderLayer != null)
                    borderLayer.Frame = new CGRect(0, Frame.Size.Height - 2.0f, Frame.Size.Width,
                        Frame.Size.Width);
            }
        }

        public override CGRect RightViewRect(CGRect forBounds)
        {
            return new CGRect(forBounds.Size.Width - 50, forBounds.Size.Height/2 - 10, 20, 20);
        }

        [Export("RightImage"), Browsable(true)]
        public string RightImage {
            set
            {
                var button = new UIButton(UIButtonType.Custom)
                {
                    Frame = new CGRect(x: 0, y: 0, width: 60, height: 40),
                    ContentMode = UIViewContentMode.ScaleAspectFit
                };
                
                button.ImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
                button.SetImage(new UIImage(value), UIControlState.Normal);

                if(ImageTouch != null)
                    button.TouchDown += ImageTouch;
                if (AfterImageTouch != null)
                    button.TouchUpInside += AfterImageTouch;
                if (ImageTouch != null && AfterImageTouch != null)
                {
                    button.Enabled = false;
                }
                RightView = button;
            }
        }

        


        [Export("ImageTouch"), Browsable(true)]
        public EventHandler ImageTouch { get; set; }

        [Export("AfterImageTouch"), Browsable(true)]
        public EventHandler AfterImageTouch { get; set; }

        public override string Placeholder
        {
            set
            {
                AttributedPlaceholder = new NSAttributedString(value, null, UIColor.White);
            }
        }

        public CGRect Shape
        {
            set
            {
                Frame = value;
                var borderLayer = Layer.Sublayers.FirstOrDefault();
                if (borderLayer != null)
                    borderLayer.Frame = new CGRect(0, Frame.Size.Height - 2.0f, Frame.Size.Width,
                        Frame.Size.Width);
            }
        }

        public TextField(IntPtr handle) : base (handle) { }

        public TextField()
        {
            // Called when created from code.
            Initialize();
        }
        
        private void Initialize()
        {
            Font = UIFont.FromName("Helvetica-Light", 13f);
            TintColor = UIColor.White;
            TextColor = UIColor.White;
            TextAlignment = UITextAlignment.Center;
            RightViewMode = UITextFieldViewMode.Always;
            HorizontalAlignment = UIControlContentHorizontalAlignment.Center;
            VerticalAlignment = UIControlContentVerticalAlignment.Center;
            BorderStyle = UITextBorderStyle.None;
            Layer.MasksToBounds = true;
            Layer.AddSublayer(new CALayer
            {
                Frame =
                    new CGRect(0, Frame.Size.Height - 2.0f, Frame.Size.Width,
                        Frame.Size.Width),
                BorderColor = UIColor.White.CGColor,
                Opacity = 0.6f,
                BorderWidth = 1.0f
            });
            
            ReturnKeyType = UIReturnKeyType.Done;
        }    
    }
}