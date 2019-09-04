using Microsoft.Azure.NotificationHubs;

namespace FESA.SCM.Notification.Models
{
    public class Notifications
    {
        //private const string HubConnectionString =
        //    "Endpoint=sb://servicio-campo.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=ndnFLJYWLaeFfXUXaAIaPwTnkTvIQEh47QlgMM29ylY=";
        private const string HubConnectionString =
        "Endpoint=sb://notification-nsc.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=J2eG0H0Ud6RjcdJIrgkfwCeyVYxC2XucYka+8iNb8ew=";

        //private const string HubPath = "fesa-notifications";
        private const string HubPath = "notification-servicio-campo";

        public static Notifications Instance = new Notifications();

        public NotificationHubClient Hub { get; set; }

        private Notifications()
        {
            Hub = NotificationHubClient.CreateClientFromConnectionString(HubConnectionString, HubPath);
        }
    }
}