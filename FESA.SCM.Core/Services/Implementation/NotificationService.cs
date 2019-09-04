using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FESA.SCM.Core.Models;
using FESA.SCM.Core.Services.Interfaces;

namespace FESA.SCM.Core.Services.Implementation
{
    public class NotificationService : INotificationService
    {
        private readonly HttpClient _httpClient;
        public NotificationService()
        {
            _httpClient = new HttpClient
            {
                MaxResponseContentBufferSize = 2147483647
            };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<string> PostToNotificationService(string token)
        {
            string registrationId = null;
            var fullMethod = $"{Constants.NotificationsRegisterUrl}?handle={token.Trim('<', '>').Replace(" ", "")}";
            var response = await _httpClient.PostAsync(fullMethod, new StringContent(""));
            if (response.IsSuccessStatusCode)
                registrationId = await response.Content.ReadAsStringAsync();
            return registrationId;
        }

        public async Task<bool> PutToNotificationService(string registrationId, string deviceRegistration)
        {
            var fullmethod = $"{Constants.NotificationsRegisterUrl}/{registrationId}";
            var response = await _httpClient.PutAsync(fullmethod, new StringContent(deviceRegistration, Encoding.UTF8, "application/json"));
            return response.IsSuccessStatusCode;
        }
    }
}