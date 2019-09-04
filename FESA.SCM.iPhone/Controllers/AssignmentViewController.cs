using System;
using System.Collections.Generic;
using CoreGraphics;
using FESA.SCM.Core.Models;
using FESA.SCM.Core.ViewModels;
using FESA.SCM.iPhone.Controls;
using FESA.SCM.iPhone.Helpers;
using FESA.SCM.iPhone.Views;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using UIKit;

namespace FESA.SCM.iPhone.Controllers
{
    public class AssignmentViewController : BaseController
    {
        private readonly INavigationService _navigationService;
        private readonly AssignmentVm _assignmentVm;
        private UITableView _activityList;
        public AssignmentViewController()
        {
            _assignmentVm = ServiceLocator.Current.GetInstance<AssignmentVm>();
            _navigationService = ServiceLocator.Current.GetInstance<INavigationService>();
        }
        
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            BuildInterface();
        }

        public override void ViewWillAppear(bool animated)
        {
            _activityList?.ReloadData();
            base.ViewWillAppear(animated);
        }

        private async void BuildInterface()
        {
            View.BackgroundColor = new UIImage("Images/fondoPrincipal.jpg").GetScaledImageBackground(View);

            await _assignmentVm.LoadActivitiesByAssignmentAsync();

            var header = new Header(View.Frame.Width)
            {
                LocationTitle = _assignmentVm.SelectedAssignment.CompanyName,
                LeftButtonImage = "Images/btn-atras.png",
                ButtonTouch = async (sender, args) =>
                {
                     await _assignmentVm.SaveAssignmentAsync();

                    _navigationService.GoBack();
                }
            };

            var principalData = new AssignmentDataView(View.Frame)
            {
                Frame = new CGRect(0, header.Frame.Y + header.Frame.Height, View.Frame.Width, 65),
            };

            var seeAssignmentInfo = new TextImageButton
            {
                Frame = new CGRect(50, principalData.Frame.Y + principalData.Frame.Height + 20, 70, 70),
                Image = "Images/datos-servicio.png",
                Content = "DATOS DE SERVICIO"
            };
            seeAssignmentInfo.TouchUpInside += (sender, args) =>
            {
                var actionSheetAlert = UIAlertController.Create("Datos de Servicio", "Seleccione que datos desea ver",
                    UIAlertControllerStyle.ActionSheet);

                actionSheetAlert.AddAction(UIAlertAction.Create("DATOS DEL SERVICIO", UIAlertActionStyle.Default,
                    (action) =>
                    {
                        _assignmentVm.SelectedServiceDataType = Enumerations.ServiceDataType.ServiceData;
                        _navigationService.NavigateTo(Constants.PageKeys.ServiceData);
                    }));
                actionSheetAlert.AddAction(UIAlertAction.Create("DATOS DE LA MAQUINA", UIAlertActionStyle.Default,
                    (action) =>
                    {
                        _assignmentVm.SelectedServiceDataType = Enumerations.ServiceDataType.MachineData;
                        _navigationService.NavigateTo(Constants.PageKeys.ServiceData);
                    }));
                actionSheetAlert.AddAction(UIAlertAction.Create("CONTACTO TÉCNICO", UIAlertActionStyle.Default,
                    (action) =>
                    {
                        _assignmentVm.SelectedServiceDataType = Enumerations.ServiceDataType.TechnicalContact;
                        _navigationService.NavigateTo(Constants.PageKeys.ServiceData);
                    }));
                actionSheetAlert.AddAction(UIAlertAction.Create("EQUIPO FERREYROS", UIAlertActionStyle.Default,
                    (action) =>
                    {
                        _assignmentVm.SelectedServiceDataType = Enumerations.ServiceDataType.FesaTeam;
                        _navigationService.NavigateTo(Constants.PageKeys.ServiceData);
                    }));

                actionSheetAlert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, (action) => { }));

                var presentationPopover = actionSheetAlert.PopoverPresentationController;
                if (presentationPopover != null)
                {
                    presentationPopover.SourceView = View;
                    presentationPopover.PermittedArrowDirections = UIPopoverArrowDirection.Up;
                }

                PresentViewController(actionSheetAlert, true, null);
            };

            var seeDocuments = new TextImageButton
            {
                Frame =
                    new CGRect(View.Frame.Width - 120, seeAssignmentInfo.Frame.Y, 70, 70),
                Image = "Images/documentos.png",
                Content = "DOCUMENTOS"
            };
            seeDocuments.TouchUpInside += (sender, args) =>
            {
                _navigationService.NavigateTo(Constants.PageKeys.Documents);
            };

            var activityView = new UIView
            {
                Frame =
                    new CGRect(0, seeAssignmentInfo.Frame.Y + seeAssignmentInfo.Frame.Height + 30, View.Frame.Width,
                        View.Frame.Height - header.Frame.Height - principalData.Frame.Height -
                        (seeAssignmentInfo.Frame.Height + 10) - 100),
                BackgroundColor = UIColor.White
            };

            var activityLabel = new UILabel
            {
                Frame = new CGRect(20, 10, 150, 30),
                Text = "Reportar Actividades",
                TextAlignment = UITextAlignment.Center,
                Font = UIFont.FromName("Helvetica-Bold",15f),
                TextColor = UIColor.Black
            };

            var addActivity = new TextImageButton
            {
                Frame = new CGRect(activityView.Frame.Width - 70, 10, 30, 30),
                Layer =
                {
                    CornerRadius = 15,
                    BackgroundColor = UIColor.Green.CGColor
                },
                ClipsToBounds = true,
                TintColor = UIColor.White,
                Font = UIFont.FromName("Helvetica-Bold", 30),
                TitleEdgeInsets = new UIEdgeInsets(-5,-1,0,0)
            };
            addActivity.SetTitle("+", UIControlState.Normal);

            _activityList = new UITableView
            {
                Frame =
                    new CGRect(0, addActivity.Frame.Y + addActivity.Frame.Height, activityView.Frame.Width,
                        activityView.Frame.Height - activityLabel.Frame.Height),
                Source = new ActivityTableSource(),
                SeparatorColor = UIColor.LightGray,
                BackgroundColor = UIColor.Clear,
                RowHeight = 60,
                PagingEnabled = true
            };

            addActivity.TouchUpInside += (sender, args) =>
            {
                if (_assignmentVm.ActiveActivity != null)
                {
                    new UIAlertView(title: "Ferreyros", message: Constants.Messages.ActivityNotFinished, del: null,
                            cancelButtonTitle: "OK", otherButtons: null).Show();
                    return;
                }
                var activity = new Activity
                {
                    Id = Guid.NewGuid().ToString(),
                    AssignmentId = _assignmentVm.SelectedAssignment.Id,
                    ActivityType = Enumerations.ActivityType.None,
                    State = Enumerations.ActivityState.New,
                    Traces = new List<Trace>()
                };

                if (_assignmentVm.SelectedAssignment.Activities == null)
                    _assignmentVm.SelectedAssignment.Activities = new List<Activity>();

                _assignmentVm.SelectedAssignment.Activities.Add(activity);
                _assignmentVm.SelectedActivity = activity;

                _activityList.ReloadData();
                _navigationService.NavigateTo(Constants.PageKeys.ActivityStart);
            };

            activityView.AddSubviews(activityLabel, addActivity, _activityList);

            var copyRight = new UILabel
            {
                Text = "2016 Ferreyros | Derechos Reservados",
                TextAlignment = UITextAlignment.Center,
                TextColor = UIColor.White,
                Font = UIFont.FromName("Helvetica-Light", 10f),
                Frame = new CGRect(0, View.Frame.Height - 30, View.Frame.Width, 20)
            };

            var closeService = new Button(UIButtonType.RoundedRect)
            {
                Content = "CERRAR SERVICIO",
                Frame = new CGRect(0, copyRight.Frame.Y - 35, View.Frame.Width, 30),
                Layer = { CornerRadius = 0},
                Font = UIFont.FromName("Helvetica-Bold", 15),
                Enabled = _assignmentVm.SelectedAssignment.Status != Enumerations.AssignmentStatus.Complete
            };

            closeService.TouchUpInside += (sender, args) =>
            {
                if (_assignmentVm.ActiveActivity != null)
                {
                    new UIAlertView(title: "Ferreyros", message: Constants.Messages.ActivityNotFinished, del: null,
                            cancelButtonTitle: "OK", otherButtons: null).Show();
                    return;
                }

                var okCancelAlertController = UIAlertController.Create("Cerrar Servicio",
                    "¿Está seguro que desea cerrar el servicio?, una vez cerrado no podrá editarlo.",
                    UIAlertControllerStyle.Alert);

                okCancelAlertController.AddAction(UIAlertAction.Create("Si", UIAlertActionStyle.Default, async alert =>
                {
                    _assignmentVm.SelectedAssignment.EndDate = DateTime.Now;
                    _assignmentVm.ActiveAssignment = null;
                    _assignmentVm.SelectedAssignment.Status = Enumerations.AssignmentStatus.Complete;
                    await _assignmentVm.SaveAssignmentAsync();
                    addActivity.Enabled = false;
                    closeService.Enabled = false;
                }));
                okCancelAlertController.AddAction(UIAlertAction.Create("No", UIAlertActionStyle.Cancel, alert => {}));

                PresentViewController(okCancelAlertController, true, null);
            };

            View.AddSubviews(header, principalData, seeAssignmentInfo, seeDocuments, activityView, closeService, copyRight);
        }

    }
}