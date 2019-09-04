using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using FESA.SCM.Notification.Models;

namespace FESA.SCM.Notification.Controllers
{
    public class NotificationsController : ApiController
    {
        [HttpPost]

        public async Task<HttpResponseMessage> Post([FromBody] FESA.SCM.Notification.Models.Notification notification)
        //string pns, [FromBody]string message //, string toTag)
        {
            var toTag = notification.toTag;
            var pns = notification.pns;
            var message = notification.message;
            var user = Guid.NewGuid().ToString();
            var userTag = new string[2];
            userTag[0] = "username:" + toTag;
            userTag[1] = "from:" + user;

            Microsoft.Azure.NotificationHubs.NotificationOutcome outcome = null;
            var ret = HttpStatusCode.InternalServerError;

            switch (pns.ToLower())
            {
                case "wns":
                    // Windows 8.1 / Windows Phone 8.1
                    var toast = @"<toast><visual><binding template=""ToastText01""><text id=""1"">" +
                                "From " + user + ": " + message + "</text></binding></visual></toast>";
                    outcome = await Notifications.Instance.Hub.SendWindowsNativeNotificationAsync(toast, userTag);
                    break;
                case "apns":
                    // iOS
                    var alert = "{\"aps\":{\"alert\":\"" + message + "\", \"sound\": \"default\", \"badge\": 1}}";
                    userTag = new string[1] { toTag };
                    outcome = await Notifications.Instance.Hub.SendAppleNativeNotificationAsync(alert, userTag);
                    break;
                case "gcm":
                    // Android
                    var notif = "{ \"data\" : {\"message\":\"" + message + "\"}}";
                    userTag = new string[1] { toTag };
                    outcome = await Notifications.Instance.Hub.SendGcmNativeNotificationAsync(notif, userTag);
                    break;
            }

            if (outcome == null) return Request.CreateResponse(ret);
            if (!((outcome.State == Microsoft.Azure.NotificationHubs.NotificationOutcomeState.Abandoned) ||
                  (outcome.State == Microsoft.Azure.NotificationHubs.NotificationOutcomeState.Unknown)))
            {
                ret = HttpStatusCode.OK;
            }

            return Request.CreateResponse(ret);
        }
    }
}