using System;
using CoreGraphics;
using FESA.SCM.Core.Models;
using UIKit;

namespace FESA.SCM.iPhone.Controls
{
    public class ActivityCell : UITableViewCell
    {
        private UILabel _activityTitle, _startDate, _activityStatus;
        public ActivityCell()
        {
            BuildInterface();
        }

        private void BuildInterface()
        {
            _activityTitle = new UILabel
            {
                TextAlignment = UITextAlignment.Left,
                TextColor = UIColor.Black,
                Font = UIFont.FromName("Helvetica-Light", 13f),
                Frame = new CGRect(10, 10, 120, 22)
            };
            _startDate = new UILabel
            {
                TextAlignment = UITextAlignment.Left,
                TextColor = UIColor.Black,
                Font = UIFont.FromName("Helvetica-Light", 13f),
                Frame = new CGRect((Frame.Width / 2) + 10, 10, 120, 22)
            };

            _activityStatus = new UILabel
            {
                TextAlignment = UITextAlignment.Left,
                TextColor = UIColor.LightGray,
                Font = UIFont.FromName("Helvetica-Light", 13f),
                Frame = new CGRect((Frame.Width / 2) + 10, _startDate.Frame.Height + 10, 120, 22)
            };
            AddSubviews(_activityTitle, _startDate, _activityStatus);
        }

        public void SetActivity(Activity activity)
        {
            _activityTitle.Text = GetTitleForActivity(activity.ActivityType);
            _startDate.Text = activity.StartDate != DateTime.MinValue
                ? $"Inicio {activity.StartDate:M}"
                : "No inciada.";
            _activityStatus.Text = GetActivityState(activity.State);
            _activityStatus.TextColor = GetStateColor(activity.State);
        }

        private string GetTitleForActivity(Enumerations.ActivityType activityType)
        {
            string activityTitle;
            switch (activityType)
            {
                case Enumerations.ActivityType.PreparingTrip:
                    activityTitle = "Preparando Viaje";
                    break;
                case Enumerations.ActivityType.Traveling:
                    activityTitle = "Viajando";
                    break;
                case Enumerations.ActivityType.Driving:
                    activityTitle = "Conduciendo";
                    break;
                case Enumerations.ActivityType.FieldService:
                    activityTitle = "Servicio en Campo";
                    break;
                case Enumerations.ActivityType.StandByClient:
                    activityTitle = "En espera por cliente";
                    break;
                case Enumerations.ActivityType.FieldReport:
                    activityTitle = "Preparando repote";
                    break;
                case Enumerations.ActivityType.StandByFesa:
                    activityTitle = "En espera por Ferreyros";
                    break;
                case Enumerations.ActivityType.None:
                    activityTitle = "Nueva actividad";
                    break;
                default:
                    activityTitle = "Nueva actividad";
                    break;
            }

            return activityTitle;
        }

        private string GetActivityState(Enumerations.ActivityState activityState)
        {
            string status;
            switch (activityState)
            {
                case Enumerations.ActivityState.Active:
                    status = "Grabando.";
                    break;
                case Enumerations.ActivityState.Completed:
                    status = "Finalizada";
                    break;
                case Enumerations.ActivityState.New:
                    status = "No iniciada";
                    break;
                default:
                    status = "No Iniciada";
                    break;
            }
            return status;
        }

        private UIColor GetStateColor(Enumerations.ActivityState activityState)
        {
            UIColor color;
            switch (activityState)
            {
                case Enumerations.ActivityState.New:
                    color = UIColor.Black;
                    break;
                case Enumerations.ActivityState.Active:
                    color = UIColor.Green;
                    break;
                case Enumerations.ActivityState.Completed:
                    color = UIColor.Red;
                    break;
                default:
                    color = UIColor.Black;
                    break;
            }
            return color;
        }
    }
}