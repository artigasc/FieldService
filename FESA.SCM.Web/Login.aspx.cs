using FESA.SCM.Web.Models;
using System;
using System.Web.UI;
using FESA.SCM.Web.Helpers;
using System.Web.UI.WebControls;
using RestSharp;
using Newtonsoft.Json;
using System.Web.Security;
using System.Web;

namespace FESA.SCM.Web
{
    public partial class _Default : Page
    {      
        private static RestApiClient _api;
        protected void Page_Load(object sender, EventArgs e)
        {
            _api = new RestApiClient(Constants.FesaApi.BaseUrl);
        }


        protected void LoginForm_Authenticate(object sender, AuthenticateEventArgs e)
        {
            Identity identidad = new Identity();
            var userId = identidad.Login(LoginForm.UserName, LoginForm.Password);
            if (userId != null)
            {

                var ticket = new FormsAuthenticationTicket(1, LoginForm.UserName, DateTime.Now, DateTime.Now.AddMinutes(FormsAuthentication.Timeout.Minutes), false, userId);
                var encryptedTicket = FormsAuthentication.Encrypt(ticket);
                var cookie = new System.Web.HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket) { Domain = FormsAuthentication.CookieDomain, Path = FormsAuthentication.FormsCookiePath, HttpOnly = true, Secure = FormsAuthentication.RequireSSL };
                HttpContext.Current.Response.Cookies.Add(cookie);

                //FormsAuthentication.SetAuthCookie(user.Name, false);
                Response.Redirect("WorkOrder.aspx");
            }
            else
                LoginForm.UserName = "";
        }
    }
}