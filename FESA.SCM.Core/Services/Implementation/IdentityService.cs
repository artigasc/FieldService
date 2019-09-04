using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FESA.SCM.Core.Helpers;
using FESA.SCM.Core.Models;
using FESA.SCM.Core.Services.Interfaces;

namespace FESA.SCM.Core.Services.Implementation
{
    public class IdentityService : IIdentityService
    {
        private readonly ApiClient _apiClient;
        public IdentityService()
        {
            _apiClient = new ApiClient(Constants.FesaApi.BaseUrl);
        }
        public async Task<User> LoginAsync(string userName, string password, CancellationToken cancellationToken = new CancellationToken())
        {
            _apiClient.AddParameter("userName", userName);
            _apiClient.AddParameter("userPassword", password);
            var response = await _apiClient.ExecutePost<User>(Constants.FesaApi.LoginMethod);
            _apiClient.CleanParameters();
            if (response.Status != HttpStatusCode.OK || response.Content == null)
            {
                return null;
            }
            var user = (User) response.Content;
            await Database.Connection.InsertOrReplaceAsync(user, cancellationToken);
            return user;
        }

        public async Task<bool> ResetPasswordAsync(string userName, CancellationToken cancellationToken = new CancellationToken())
        {
            _apiClient.AddParameter("userName", userName);
            var response = await _apiClient.ExecutePost(Constants.FesaApi.RestorePasswordMethod);
            _apiClient.CleanParameters();
            return response.Status == HttpStatusCode.OK;
        }

        public async Task<bool> ChangePasswordAsync(string userId, string newPassword, CancellationToken cancellationToken = new CancellationToken())
        {
            _apiClient.AddParameter("userId", userId);
            _apiClient.AddParameter("userPassword", newPassword);
            var response = await _apiClient.ExecutePost(Constants.FesaApi.ChangePasswordMethod);
            _apiClient.CleanParameters();
            return response.Status == HttpStatusCode.OK;
        }
        

        public async Task<User> GetUserDataAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return await Database.Connection.Table<User>().FirstAsync(cancellationToken);
        }

        public async Task UpdateUserDataAsync(Enumerations.UserStatus status, CancellationToken cancellationToken = new CancellationToken())
        {
            var user = await Database.Connection.Table<User>().FirstAsync(cancellationToken);
            user.UserStatus = status;
            await Database.Connection.UpdateAsync(user, cancellationToken);
        }

        public async Task<byte[]> DownloadUserImage(string imageUrl)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetByteArrayAsync(imageUrl);
                return response;
            }
        }

        public async Task LogOffAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var user = await Database.Connection.Table<User>().FirstAsync(cancellationToken);
            _apiClient.AddParameter("userId", user.Id);
            var response = await _apiClient.ExecuteGet(Constants.FesaApi.LogOff);
            _apiClient.CleanParameters();
            Database.ClearDataBase(cancellationToken);
        }
    }
}