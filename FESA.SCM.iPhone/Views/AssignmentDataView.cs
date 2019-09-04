using System;
using CoreGraphics;
using FESA.SCM.Core.Models;
using FESA.SCM.Core.ViewModels;
using Microsoft.Practices.ServiceLocation;
using UIKit;

namespace FESA.SCM.iPhone.Views
{
    public class AssignmentDataView : UIView
    {
        private readonly AssignmentVm _assignmentVm;
        private readonly CGRect _bounds;
        public AssignmentDataView(CGRect bounds)
        {
            _assignmentVm = ServiceLocator.Current.GetInstance<AssignmentVm>();
            _bounds = bounds;
            BuildInterface();
        }

        private void BuildInterface()
        {

            BackgroundColor = UIColor.White;
            var workOrderLabel = new UILabel
            {
                Text = $"OT. {_assignmentVm.SelectedAssignment.WorkOrderNumber}",
                Frame = new CGRect(40, 10, 120, 22),
                TextColor = UIColor.Black,
                Font = UIFont.FromName("Helvetica-Light", 12),
                TextAlignment = UITextAlignment.Left
            };

            var machineSerialLabel = new UILabel
            {
                Text = $"N/S: {_assignmentVm.SelectedAssignment.Machine.SerialNumber}",
                Frame = new CGRect(40, workOrderLabel.Frame.Height + 15, 120, 22),
                TextColor = UIColor.Black,
                Font = UIFont.FromName("Helvetica-Light", 12),
                TextAlignment = UITextAlignment.Left
            };

            var startDateLabel = new UILabel
            {
                Text = $"Inicio {_assignmentVm.SelectedAssignment.StartDate:MM/dd}",
                Frame = new CGRect(workOrderLabel.Frame.Width + 20, 10, 120, 22),
                TextColor = UIColor.Black,
                Font = UIFont.FromName("Helvetica-Light", 12),
                TextAlignment = UITextAlignment.Right
            };

            var serviceStatusLabel = new UILabel
            {
                Text = GetTextFromStatus(_assignmentVm.SelectedAssignment.Status),
                Frame = new CGRect(startDateLabel.Frame.Width + 20, startDateLabel.Frame.Height + 15, 120, 22),
                TextColor = GetColorFromStatus(_assignmentVm.SelectedAssignment.Status),
                Font = UIFont.FromName("Helvetica-Light", 13),
                TextAlignment = UITextAlignment.Right
            };

            AddSubviews(workOrderLabel, machineSerialLabel, startDateLabel, serviceStatusLabel);
        }

        private string GetTextFromStatus(Enumerations.AssignmentStatus status)
        {
            string statusText;

            switch (status)
            {
                case Enumerations.AssignmentStatus.New:
                    statusText = "No Iniciado";
                    break;
                case Enumerations.AssignmentStatus.Hold:
                    statusText = "En Espera";
                    break;
                case Enumerations.AssignmentStatus.Active:
                    statusText = "En Progreso";
                    break;
                case Enumerations.AssignmentStatus.Complete:
                case Enumerations.AssignmentStatus.Declined:
                    statusText = "Completo";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }

            return statusText;
        }

        private UIColor GetColorFromStatus(Enumerations.AssignmentStatus status)
        {
            UIColor color;
            switch (status)
            {
                case Enumerations.AssignmentStatus.New:
                    color = UIColor.Red;
                    break;
                case Enumerations.AssignmentStatus.Hold:
                    color = UIColor.FromRGB(249, 195, 22);
                    break;
                case Enumerations.AssignmentStatus.Active:
                    color = UIColor.Green;
                    break;
                case Enumerations.AssignmentStatus.Complete:
                case Enumerations.AssignmentStatus.Declined:
                    color = UIColor.Black;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
            return color;
        }
    }
}