using System;
using FESA.SCM.Core.Models;
using UIKit;

namespace FESA.SCM.iPhone.Controls
{
    public class StatusImage : UIImageView
    {
        public StatusImage()
        {
        }

        public void SetImage(Enumerations.AssignmentStatus status)
        {
            string file;

            switch (status)
            {
                case Enumerations.AssignmentStatus.New:
                case Enumerations.AssignmentStatus.Active:
                    file = "green";
                    break;
                case Enumerations.AssignmentStatus.Hold:
                    file = "yellow";                     
                    break;
                case Enumerations.AssignmentStatus.Complete:
                case Enumerations.AssignmentStatus.Declined:
                    file = "red";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }

            Image = new UIImage($"Images/{file}.png");
        }
        
    }
}