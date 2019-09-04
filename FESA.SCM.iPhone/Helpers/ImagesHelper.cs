using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using FESA.SCM.Core.Services.Interfaces;
using Foundation;
using Microsoft.Practices.ServiceLocation;
using UIKit;

namespace FESA.SCM.iPhone.Helpers
{
    public static class ImagesHelper
    {
        public static UIColor GetScaledImageBackground(this UIImage image, UIView view)
        {
            UIGraphics.BeginImageContext(view.Frame.Size);
            image.Draw(view.Frame);
            image = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return UIColor.FromPatternImage(image);
        }

        public static async Task<UIImage> GetImageFromUrl(this string imageUrl)
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var library = Path.Combine(documentsPath, "..", "Library");
            var pngFilename = Path.Combine(library, "user_photo.png"); // hardcoded filename, overwritten each time
            if (File.Exists(pngFilename)) return UIImage.FromFile(pngFilename);
            var identityService = ServiceLocator.Current.GetInstance<IIdentityService>();
            var imageByteArray = await identityService.DownloadUserImage(imageUrl);
            File.WriteAllBytes(pngFilename, imageByteArray);
            return UIImage.FromFile(pngFilename);
        }
    }
}