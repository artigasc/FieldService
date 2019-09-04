using CoreGraphics;
using FESA.SCM.Core.Models;
using FESA.SCM.iPhone.Helpers;
using Foundation;
using UIKit;

namespace FESA.SCM.iPhone.Controls
{
    public class AssignmentHistoryCell : UITableViewCell
    {
        private UILabel _startDate, _endDate, _companyName, _serviceType;
        private StatusImage _signalView;

        public AssignmentHistoryCell()
        {
            BuildInterface();
        }

        private void BuildInterface()
        {
            BackgroundColor = UIColor.White;
            Frame = new CGRect(0, 0, Frame.Width, 80);
            SelectedBackgroundView = new UIImageView
            {
                Image = UIImage.FromFile("Images/assignmentactiveblue.png")
            };

            _signalView = new StatusImage
            {
                Frame = new CGRect(10, 15, 20, 20)
            };

            _startDate = new UILabel
            {
                TextAlignment = UITextAlignment.Left,
                TextColor = UIColor.Black,
                Font = UIFont.FromName("Helvetica-Light", 13f),
                Frame = new CGRect(_signalView.Frame.Width + 20, 10, 120, 22)
            };

            _endDate = new UILabel
            {
                TextAlignment = UITextAlignment.Left,
                TextColor = UIColor.Gray,
                Font = UIFont.FromName("Helvetica-Light", 11f),
                Frame = new CGRect(_signalView.Frame.Width + 20, _startDate.Frame.Height + 5, 120, 22)
            };

            _companyName = new UILabel
            {
                TextAlignment = UITextAlignment.Right,
                TextColor = UIColor.Black,
                Font = UIFont.FromName("Helvetica-Light", 13f),
                Frame = new CGRect(_startDate.Frame.Width + 20f, 10, 120, 22),
                LineBreakMode = UILineBreakMode.WordWrap
            };

            _serviceType = new UILabel
            {
                TextAlignment = UITextAlignment.Right,
                TextColor = UIColor.Gray,
                Font = UIFont.FromName("Helvetica-Light", 11f),
                Frame = new CGRect(_startDate.Frame.Width + 20f, _companyName.Frame.Height + 5, 120, 22)
            };

            AddSubviews(_signalView, _startDate, _endDate, _companyName, _serviceType);
        }

        public void SetAssignment(Assignment assignment, NSIndexPath index)
        {
            _startDate.Text = $"Inicio {assignment.EstimatedStartDate:dd/MM}";
            _endDate.Text = $"Fin {assignment.EndDate:dd/MM}";
            _companyName.Text = assignment.CompanyName;
            _serviceType.Text = Utils.GetServiceTypeText(assignment.AssignmentType);
            _signalView.SetImage(assignment.Status);

        }
    }
}