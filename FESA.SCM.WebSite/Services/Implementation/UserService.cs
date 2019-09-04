using FESA.SCM.WebSite.Helpers;
using FESA.SCM.WebSite.Models;
using FESA.SCM.WebSite.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FESA.SCM.WebSite.Services.Implementation
{
    public class UserService :IUserService
    {
        private ApiClient _apiClient;

        public UserService() {
            _apiClient = new ApiClient(Constants.UrlBase);
        }

        public async Task<UserModel> Login(string username, string password) {
            UserModel item = null;
            try {
                _apiClient.AddParameter("username", username);
                _apiClient.AddParameter("userpassword", password);
                var response = await _apiClient.ExecutePost<UserModel>("Users/Login");
                if (response == null || response.Status != System.Net.HttpStatusCode.OK) {
                    return null;
                }
                item = (UserModel)response.Content;
            } catch (Exception e) {
                string m = e.Message;
            }
            return item;
        }
    }
}
