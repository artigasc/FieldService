using System;
using CoreGraphics;
using Syncfusion.SfBusyIndicator.iOS;
using UIKit;

namespace FESA.SCM.iPhone.Controls
{
    public sealed class LoadingOverlay : UIView
    {
        public LoadingOverlay(CGRect frame): base(frame)
        {
            BackgroundColor = UIColor.Black;
            Alpha = 0.75f;
            AutoresizingMask = UIViewAutoresizing.All;

            nfloat labelHeight = 22;
            var labelWidth = Frame.Width - 20;

            var centerX = Frame.Width / 2;
            var centerY = Frame.Height / 2;

            var busyIndicator = new SFBusyIndicator
            {
                Duration = 0.6f,
                Frame = new CGRect(0, 0, Frame.Size.Width, Frame.Size.Height),
                ViewBoxWidth = 70,
                ViewBoxHeight = 70,
                Foreground = UIColor.White,
                BackgroundColor = UIColor.Clear,
                AnimationType = SFBusyIndicatorAnimationType.SFBusyIndicatorAnimationTypeDoubleCircle
               
            };
            AddSubview(busyIndicator);

            var loadingLabel = new UILabel(new CGRect(
                centerX - (labelWidth/2) + 5,
                centerY + 45,
                labelWidth,
                labelHeight
                ))
            {
                BackgroundColor = UIColor.Clear,
                TextColor = UIColor.White,
                Text = "Cargando...",
                TextAlignment = UITextAlignment.Center,
                AutoresizingMask = UIViewAutoresizing.All
            };
            AddSubview(loadingLabel);
        }

        public void Hide()
        {
            Animate(
                duration: 0.5, 
                animation: () => { Alpha = 0; },
                completion: RemoveFromSuperview
            );
        }
    }
}