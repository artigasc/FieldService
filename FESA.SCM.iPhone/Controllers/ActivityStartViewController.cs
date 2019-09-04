using System;
using CoreGraphics;
using CoreLocation;
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
    public class ActivityStartViewController : BaseController
    {
        private readonly INavigationService _navigationService;
        private readonly AssignmentVm _assignmentVm;
        private ActivityFill _description, _activityType;
        private ActivityDate _startDate, _endDate;
        private UILabel _timespan;
        private UIView _timerView;
        private readonly NetworkStatus _networkStatus;
        private double _latitude;
        private double _longitude;
        private CLLocationManager _locationManager;

        public ActivityStartViewController()
        {
            _navigationService = ServiceLocator.Current.GetInstance<INavigationService>();
            _assignmentVm = ServiceLocator.Current.GetInstance<AssignmentVm>();
            _assignmentVm.RecordingChanged += OnRecordingChanged;
            _assignmentVm.TimerChanged += OnTimerChanged;
            _networkStatus = Reachability.InternetConnectionStatus();
            BuildInterface();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            if (!CLLocationManager.LocationServicesEnabled) return;
            _locationManager = new CLLocationManager();
            _locationManager.PausesLocationUpdatesAutomatically = false;

            _locationManager.AllowsBackgroundLocationUpdates = true;
            _locationManager.DesiredAccuracy = 1;
            _locationManager.LocationsUpdated += OnLocationsUpdated;
            _locationManager.StartUpdatingLocation();
        }

        private void OnLocationsUpdated(object sender, CLLocationsUpdatedEventArgs args)
        {
            var currentLocation = args.Locations[args.Locations.Length - 1];
            _latitude = currentLocation.Coordinate.Latitude;
            _longitude = currentLocation.Coordinate.Longitude;
        }

        private void BuildInterface()
        {
            View.BackgroundColor = new UIImage("Images/fondoPrincipal.jpg").GetScaledImageBackground(View);

            var header = new Header(View.Frame.Width)
            {
                LocationTitle = _assignmentVm.SelectedAssignment.CompanyName,
                LeftButtonImage = "Images/btn-atras.png",
                ButtonTouch = async (sender, args) =>
                {
                    if (_assignmentVm.SelectedActivity.ActivityType == Enumerations.ActivityType.None)
                    {
                        _assignmentVm.SelectedAssignment.Activities.Remove(_assignmentVm.SelectedActivity);
                    }
                    else
                    {
                        if (_assignmentVm.SelectedActivity.IsRecording)
                        {
                            await _assignmentVm.StopAsync(willContinue: true);
                            _assignmentVm.SelectedActivity.IsRecording = true;
                        }
                        _assignmentVm.SelectedActivity.Traces.Add(new Trace
                        {
                            ActivityId = _assignmentVm.SelectedActivity.Id,
                            ActivityState = _assignmentVm.SelectedActivity.State,
                            TraceDate = DateTime.Now,
                            Longitude = (decimal)_longitude,
                            Latitude = (decimal)_latitude
                        });
                        await _assignmentVm.SaveActivityAsync();
                    }
                    _navigationService.GoBack();
                }
            };

            var principalData = new AssignmentDataView(View.Frame)
            {
                Frame = new CGRect(0, header.Frame.Y + header.Frame.Height, View.Frame.Width, 65)
            };

            _activityType = new ActivityFill
            {
                Title = "Tipo de Actividad",
                BackgroundColor = UIColor.Clear,
                Frame = new CGRect(10, principalData.Frame.Y + principalData.Frame.Height, View.Frame.Width, 40),
                Hit = TouchActivityTypeButton
            };

            _description = new ActivityFill
            {
                Title = "Decripción",
                BackgroundColor = UIColor.Clear,
                Frame = new CGRect(10, _activityType.Frame.Y + _activityType.Frame.Height, View.Frame.Width - 20, 40),
                Hit = TouchButtonDescription
            };

            _startDate = new ActivityDate
            {
                Title = "Fecha de Inicio",
                Frame = new CGRect(10, _description.Frame.Y + _description.Frame.Height, View.Frame.Width, 40)
            };

            _endDate = new ActivityDate
            {
                Title = "Fecha de Inicio",
                Frame = new CGRect(10, _startDate.Frame.Y + _startDate.Frame.Height, View.Frame.Width, 40)
            };

            _description.NewTitle = string.IsNullOrEmpty(_assignmentVm.SelectedActivity.Description)
                ? "+"
                : _assignmentVm.SelectedActivity.Description;
            _activityType.NewTitle = GetTypeName(_assignmentVm.SelectedActivity.ActivityType);
            _startDate.Date = (_assignmentVm.SelectedActivity.StartDate != DateTime.MinValue)
                ? _assignmentVm.SelectedActivity.StartDate.ToString("dd/MM/yyyy hh:mm")
                : string.Empty;
            _endDate.Date = (_assignmentVm.SelectedActivity.EndDate != DateTime.MinValue)
                ? _assignmentVm.SelectedAssignment.EndDate.ToString("dd/MM/yyyy hh:mm")
                : string.Empty;
            
            if (_assignmentVm.SelectedActivity.IsRecording)
            {
                var elapsed = DateTime.Now - _assignmentVm.SelectedActivity.StartDate;
                _assignmentVm.SelectedActivity.Duration = _assignmentVm.SelectedActivity.Duration.Add(elapsed);
                _assignmentVm.RecordAsync();
            }

            _timerView = new UIView
            {
                Frame = new CGRect(View.Frame.Width/2 - 70, _endDate.Frame.Y + _endDate.Frame.Height + 10, 160, 160)
            };

            _timespan = new UILabel
            {
                Frame = new CGRect(0, 0, 160, 60),
                Font = UIFont.FromName("Helvetica-Bold", 35),
                TextColor = UIColor.White,
                Text = _assignmentVm.SelectedActivity.Duration.ToString(@"hh\:mm\:ss")
            };

            var timerButton = UIButton.FromType(UIButtonType.Custom);
            timerButton.Frame = new CGRect(View.Frame.Width / 2 - 120, _timespan.Frame.Height + 5, 70, 70);
            timerButton.SetImage(
                    _assignmentVm.SelectedActivity.IsRecording
                        ? new UIImage("Images/btn-stop.png")
                        : new UIImage("Images/btn-play.png"), UIControlState.Normal);

            timerButton.Enabled = _assignmentVm.SelectedAssignment.Status != Enumerations.AssignmentStatus.Complete;
            timerButton.TouchUpInside += (sender, args) =>
            {
                if (_assignmentVm.ActiveActivity != null &&
                    _assignmentVm.SelectedActivity.Id != _assignmentVm.ActiveActivity.Id)
                {
                    new UIAlertView(title: "Ferreyros", message: Constants.Messages.ActivityNotFinished, del: null,
                        cancelButtonTitle: "OK", otherButtons: null).Show();
                    return;
                }
                _activityType.UserInteractionEnabled = false;
                _description.UserInteractionEnabled = false;
                
                timerButton.Enabled = false;
                _assignmentVm.ActiveActivity = !_assignmentVm.SelectedActivity.IsRecording
                    ? _assignmentVm.SelectedActivity
                    : null;

                var task = _assignmentVm.SelectedActivity.IsRecording
                    ? _assignmentVm.StopAsync()
                    : _assignmentVm.RecordAsync();

                timerButton.SetImage(
                    _assignmentVm.SelectedActivity.IsRecording
                        ? new UIImage("Images/btn-stop.png")
                        : new UIImage("Images/btn-play.png"), UIControlState.Normal);

                task.ContinueWith(_ =>
                {
                    BeginInvokeOnMainThread(() =>
                    {
                        timerButton.Enabled = _assignmentVm.ActiveActivity != null;
                    });
                });
            };
            //_timerView.Hidden = _assignmentVm.SelectedActivity.ActivityType == Enumerations.ActivityType.None;
            _timerView.AddSubviews(timerButton, _timespan);

            var notifyClient = UIButton.FromType(UIButtonType.Custom);
            notifyClient.SetImage(new UIImage("Images/send-mail.png"), UIControlState.Normal);
            notifyClient.SetTitle("Notificar el fin del servicio al Cliente", UIControlState.Normal);
            notifyClient.BackgroundColor = UIColor.Clear;
            notifyClient.TitleLabel.Font = UIFont.FromName("Helvetica-Light", 13f);
            notifyClient.TitleLabel.TextAlignment = UITextAlignment.Left;
            notifyClient.TitleLabel.LineBreakMode = UILineBreakMode.WordWrap;
            notifyClient.TitleEdgeInsets = new UIEdgeInsets(0, -60, 0, -150);
            notifyClient.Frame = new CGRect(40, _timerView.Frame.Y + _timerView.Frame.Height + 10, 80, 40);
            notifyClient.ImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
            //notifyClient.Enabled = _assignmentVm.SelectedActivity.State != Enumerations.ActivityState.Completed;
            notifyClient.TouchUpInside += (sender, args) =>
            {
                if (_assignmentVm.SelectedActivity.IsRecording)
                {
                    new UIAlertView(title: "Ferreyros", message: Constants.Messages.ActivityNotFinished, del: null,
                            cancelButtonTitle: "OK", otherButtons: null).Show();
                    return;
                }

                var okCancelAlertController = UIAlertController.Create("Cerrar Servicio",
                    "¿Con esta actividad se ha completado el servicio?, se enviará un correo al cliente informando.",
                    UIAlertControllerStyle.Alert);

                okCancelAlertController.AddAction(UIAlertAction.Create("Si", UIAlertActionStyle.Default, async alert =>
                {
                    _assignmentVm.SelectedActivity.State = Enumerations.ActivityState.Completed;
                    await _assignmentVm.SaveActivityAsync();
                    await _assignmentVm.SendActivityEndsAssignmentMailAsync();
                    notifyClient.Enabled = false;
                }));
                okCancelAlertController.AddAction(UIAlertAction.Create("No", UIAlertActionStyle.Cancel, alert => { }));

                PresentViewController(okCancelAlertController, true, null);
            };

            var copyRight = new UILabel
            {
                Text = "2016 Ferreyros | Derechos Reservados",
                TextAlignment = UITextAlignment.Center,
                TextColor = UIColor.White,
                Font = UIFont.FromName("Helvetica-Light", 10f),
                Frame = new CGRect(0, View.Frame.Height - 30, View.Frame.Width, 20)
            };

            View.AddSubviews(header, principalData, _activityType, _description, _startDate, _endDate, _timerView,
                notifyClient, copyRight);
        }

        private void TouchActivityTypeButton(object sender, EventArgs eventArgs)
        {
            var actionSheetAlert = UIAlertController.Create("Tipo de Actividad",
                "Seleccione el tipo de actividad a realizar",
                UIAlertControllerStyle.ActionSheet);

            actionSheetAlert.AddAction(UIAlertAction.Create("PREPARANDO VIAJE", UIAlertActionStyle.Default,
                action =>
                {
                    _assignmentVm.SelectedActivity.ActivityType = Enumerations.ActivityType.PreparingTrip;
                    _activityType.NewTitle = GetTypeName(Enumerations.ActivityType.PreparingTrip);
                }));
            actionSheetAlert.AddAction(UIAlertAction.Create("VIAJANDO", UIAlertActionStyle.Default,
                async action =>
                {
                    _assignmentVm.SelectedActivity.ActivityType = Enumerations.ActivityType.Traveling;
                    _activityType.NewTitle = GetTypeName(Enumerations.ActivityType.Traveling);
                    if(_networkStatus != NetworkStatus.NotReachable)
                        await _assignmentVm.SendActivityMailAsync();
                }));
            actionSheetAlert.AddAction(UIAlertAction.Create("MANEJANDO", UIAlertActionStyle.Default,
                async action =>
                {
                    _assignmentVm.SelectedActivity.ActivityType = Enumerations.ActivityType.Driving;
                    _activityType.NewTitle = GetTypeName(Enumerations.ActivityType.Driving);
                    if(_networkStatus != NetworkStatus.NotReachable)
                        await _assignmentVm.SendActivityMailAsync();
                }));
            actionSheetAlert.AddAction(UIAlertAction.Create("SERVICIO DE CAMPO", UIAlertActionStyle.Default,
                action =>
                {
                    _assignmentVm.SelectedActivity.ActivityType = Enumerations.ActivityType.FieldService;
                    _activityType.NewTitle = GetTypeName(Enumerations.ActivityType.FieldService);
                }));

            actionSheetAlert.AddAction(UIAlertAction.Create("ESPERA POR CLIENTE", UIAlertActionStyle.Default,
                async action =>
                {
                    _assignmentVm.SelectedActivity.ActivityType = Enumerations.ActivityType.StandByClient;
                    _activityType.NewTitle = GetTypeName(Enumerations.ActivityType.StandByClient);
                    if (_networkStatus != NetworkStatus.NotReachable)
                        await _assignmentVm.SendActivityMailAsync();
                }));

            actionSheetAlert.AddAction(UIAlertAction.Create("ELABORACIÓN DE INFORME",
                UIAlertActionStyle.Default,
                action =>
                {
                    _assignmentVm.SelectedActivity.ActivityType = Enumerations.ActivityType.FieldReport;
                    _activityType.NewTitle = GetTypeName(Enumerations.ActivityType.FieldReport);
                }));

            actionSheetAlert.AddAction(UIAlertAction.Create("ESPERA FESA", UIAlertActionStyle.Default,
                async action =>
                {
                    _assignmentVm.SelectedActivity.ActivityType = Enumerations.ActivityType.StandByFesa;
                    _activityType.NewTitle = GetTypeName(Enumerations.ActivityType.StandByFesa);
                    if(_networkStatus != NetworkStatus.NotReachable)
                        await _assignmentVm.SendActivityMailAsync();
                }));

            actionSheetAlert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, action => { }));

            var presentationPopover = actionSheetAlert.PopoverPresentationController;
            if (presentationPopover != null)
            {
                presentationPopover.SourceView = View;
                presentationPopover.PermittedArrowDirections = UIPopoverArrowDirection.Up;
            }

            PresentViewController(actionSheetAlert, true, null);
        }

        private string GetTypeName(Enumerations.ActivityType type)
        {
            string typeName;
            switch (type)
            {
                case Enumerations.ActivityType.PreparingTrip:
                    typeName = "Preparando Viaje";
                    break;
                case Enumerations.ActivityType.Traveling:
                    typeName = "Viajando";
                    break;
                case Enumerations.ActivityType.Driving:
                    typeName = "Manejando";
                    break;
                case Enumerations.ActivityType.FieldService:
                    typeName = "Servicio de Campo";
                    break;
                case Enumerations.ActivityType.StandByClient:
                    typeName = "Espera por cliente";
                    break;
                case Enumerations.ActivityType.FieldReport:
                    typeName = "Elaboración de informe de campo";
                    break;
                case Enumerations.ActivityType.StandByFesa:
                    typeName = "Espera FESA";
                    break;
                case Enumerations.ActivityType.None:
                    typeName = "+";
                    break;
                default:
                    typeName = "+";
                    break;
            }
            //_timerView.Hidden = typeName.Equals("+");
            return typeName;
        }

        private void TouchButtonDescription(object sender, EventArgs eventArgs)
        {
            var textInputAlertController = UIAlertController.Create("Descripción", "Ingrese la descripción de la actividad", UIAlertControllerStyle.Alert);


            textInputAlertController.AddTextField(textField =>
            {
                textField.Layer.CornerRadius = 8;
                textField.Placeholder = "Actividad";
                textField.SetDidChangeNotification(text => _assignmentVm.SelectedActivity.Description = text.Text);
            });

            //var textfield = new UITextView
            //{
            //    Frame = new CGRect(),
            //    Text = "Actividad",
            //    Editable = true
            //};
            //textfield.Changed += (o, args) => { _assignmentVm.SelectedActivity.Description = textfield.Text; };
            //textInputAlertController.View.AddSubviews(textfield);

            var cancelAction = UIAlertAction.Create("Cancelar", UIAlertActionStyle.Cancel, alertAction => { _assignmentVm.SelectedActivity.Description = _description.Title; });
            var okayAction = UIAlertAction.Create("Aceptar", UIAlertActionStyle.Default, alertAction => { _description.NewTitle = _assignmentVm.SelectedActivity.Description; });

            textInputAlertController.AddAction(cancelAction);
            textInputAlertController.AddAction(okayAction);

            PresentViewController(textInputAlertController, true, null);
        }

        private void OnRecordingChanged(object sender, EventArgs eventArgs)
        {
            if (_assignmentVm.SelectedActivity.IsRecording)
                _startDate.Date = _assignmentVm.SelectedActivity.StartDate.ToString("dd/MM/yyyy hh:mm");
            else
                _endDate.Date = _assignmentVm.SelectedActivity.EndDate.ToString("dd/MM/yyyy  hh:mm");
        }

        private void OnTimerChanged(object sender, EventArgs eventArgs)
        {
            _timespan.Text = _assignmentVm.SelectedActivity.Duration.ToString(@"hh\:mm\:ss");
        }

        protected override void Dispose(bool disposing)
        {
            _assignmentVm.RecordingChanged -= OnRecordingChanged;
            _assignmentVm.TimerChanged -= OnTimerChanged;
            base.Dispose(disposing);
        }
    }
}