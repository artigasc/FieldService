using FESA.SCM.Web.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using RestSharp;
using System;
using System.Web.Security;

namespace FESA.SCM.Web.Helpers
{
    public class Identity
    {
        HttpClient Client;
        public Identity()
        {
            Client = new HttpClient();
            Client.MaxResponseContentBufferSize = 256000;
        }

        public string Login(string username, string password)
        {
            //ANTONIO
            var client = new RestClient("http://apiprod-gateway.azurewebsites.net/api");
            //var client = new RestClient("http://apitest-gateway.azurewebsites.net/api");
            var request = new RestRequest("Users/Login");
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(new
            {
                UserName = username,
                UserPassword = password
            });

            var response = client.ExecutePostTaskAsync(request).GetAwaiter().GetResult();
            
            var loginRaw = JsonConvert.DeserializeObject<User>(response.Content);
            

            if (response.StatusCode != HttpStatusCode.OK || response.Content == null || response.Content == "null")
            {
                return null;
            }
            else
            {
                var User = new User
                {
                    Id = loginRaw.Id,
                    UserType = loginRaw.UserType
                };
                if (User.UserType == Enumerations.UserType.Technician)
                    return null;

                return User.Id;
            }
           
        }
    }
}