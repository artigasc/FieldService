using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using FESA.SCM.Notification.Models;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Azure.NotificationHubs.Messaging;
using RestSharp;

namespace FESA.SCM.Notification.Controllers
{
    public class RegisterController : ApiController
    {
        private readonly NotificationHubClient _hub;

        public RegisterController()
        {
            _hub = Notifications.Instance.Hub;
        }

        public class DeviceRegistration
        {
            public string Platform { get; set; }
            public string Handle { get; set; }
            public string[] Tags { get; set; }
            public string UserId { get; set; }
        }

        // POST api/register
        public async Task<string> Post(string handle = null)
        {
            string newRegistrationId = null;

            if (handle == null) return await _hub.CreateRegistrationIdAsync();
            var registrations = await _hub.GetRegistrationsByChannelAsync(handle, 100);

            foreach (var registration in registrations)
            {
                if (newRegistrationId == null)
                {
                    newRegistrationId = registration.RegistrationId;
                }
                else
                {
                    await _hub.DeleteRegistrationAsync(registration);
                }
            }

            return newRegistrationId ?? (await _hub.CreateRegistrationIdAsync());
        }

        // PUT api/register/5
        // This creates or updates a registration (with provided channelURI) at the specified id
        public async Task<HttpResponseMessage> Put(string id, DeviceRegistration deviceUpdate)
        {
            RegistrationDescription registration;
            switch (deviceUpdate.Platform)
            {
                case "mpns":
                    registration = new MpnsRegistrationDescription(deviceUpdate.Handle);
                    break;
                case "wns":
                    registration = new WindowsRegistrationDescription(deviceUpdate.Handle);
                    break;
                case "apns":
                    registration = new AppleRegistrationDescription(deviceUpdate.Handle);
                    break;
                case "gcm":
                    registration = new GcmRegistrationDescription(deviceUpdate.Handle);
                    break;
                default:
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            registration.RegistrationId = id;
            var username = deviceUpdate.UserId;

            registration.Tags = new HashSet<string>(deviceUpdate.Tags) {"username:" + username};

            try
            {
                await _hub.CreateOrUpdateRegistrationAsync(registration);
                await SetPnsToUser(deviceUpdate.UserId, deviceUpdate.Platform);
            }
            catch (MessagingException e)
            {
                ReturnGoneIfHubResponseIsGone(e);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE api/register/5
        public async Task<HttpResponseMessage> Delete(string id)
        {
            await _hub.DeleteRegistrationAsync(id);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private async Task SetPnsToUser(string userId, string pns)
        {
            var url = ConfigurationManager.AppSettings["identity-api"];
            var client = new RestClient(url);
            var request = new RestRequest("setuserpns");
            request.AddJsonBody(new
            {
                userId,
                pns
            });
            var response = await client.ExecutePostTaskAsync(request);
            if(response.StatusCode != HttpStatusCode.OK)
                throw new HttpRequestException("Bad Request");
        }

        private static void ReturnGoneIfHubResponseIsGone(MessagingException e)
        {
            var webex = e.InnerException as WebException;
            if (webex == null || webex.Status != WebExceptionStatus.ProtocolError) return;
            var response = (HttpWebResponse)webex.Response;
            if (response.StatusCode == HttpStatusCode.Gone)
                throw new HttpRequestException(HttpStatusCode.Gone.ToString());
        }
    }
}
