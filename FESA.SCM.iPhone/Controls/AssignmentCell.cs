using System;
using System.Globalization;
using CoreGraphics;
using FESA.SCM.Core.Helpers;
using FESA.SCM.Core.Models;
using FESA.SCM.Core.ViewModels;
using FESA.SCM.iPhone.Controllers;
using FESA.SCM.iPhone.Helpers;
using Foundation;
using Microsoft.Practices.ServiceLocation;
using UIKit;

namespace FESA.SCM.iPhone.Controls
{
    public class AssignmentCell : UITableViewCell
    {
        private UILabel _startDate, _workORder, _companyName, _serviceType;
        private StatusImage _signalView;

        public AssignmentCell()
        {
            BuildInterface();
        }

        private void BuildInterface()
        {
            BackgroundColor = UIColor.White;
            Frame = new CGRect(0,0,Frame.Width, 80);
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

            _workORder = new UILabel
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
                Frame = new CGRect(_startDate.Frame.Width + 20f, 10, 140, 22),
                LineBreakMode = UILineBreakMode.WordWrap
            };

            _serviceType = new UILabel
            {
                TextAlignment = UITextAlignment.Right,
                TextColor = UIColor.Gray,
                Font = UIFont.FromName("Helvetica-Light", 11f),
                Frame = new CGRect(_startDate.Frame.Width + 20f, _companyName.Frame.Height + 5, 140, 22)
            };

            AddSubviews(_signalView, _startDate, _workORder, _companyName, _serviceType);
        }

        public void SetAssignment(Assignment assignment, NSIndexPath index)
        {
            _startDate.Text = $"Inicio {assignment.EstimatedStartDate:dd/MM}";
            _workORder.Text = $"OT. {assignment.WorkOrderNumber.ToUpper()}";
            _companyName.Text = assignment.CompanyName;
            _serviceType.Text = Utils.GetServiceTypeText(assignment.AssignmentType);
            _signalView.SetImage(assignment.Status);

        }
    }
}