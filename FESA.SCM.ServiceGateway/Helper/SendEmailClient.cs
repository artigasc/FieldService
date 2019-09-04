using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;

namespace FESA.SCM.ServiceGateway.Helper
{
    public class SendEmailClient
    {
        private string MailAccount { get; set; }
        private string MailPassword { get; set; }
        private string SMTP { get; set; }
        private string MailDisplayName { get; set; }
        private string SystemUrl { get; set; }

        public SendEmailClient()
        {
            MailAccount = ConfigurationManager.AppSettings["mailAccount"];
            MailPassword = ConfigurationManager.AppSettings["mailPassword"];
            SMTP = ConfigurationManager.AppSettings["smtp"];
            MailDisplayName = ConfigurationManager.AppSettings["mailDisplayName"];
            SystemUrl = ConfigurationManager.AppSettings["systemUrl"];
        }

        public void PostSendEmail(List<string> userNames, string subject, string body)
        {
            var smtpClient = new SmtpClient
            {
                Host = SMTP,
                Port = 587,
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(MailAccount, MailPassword),
                EnableSsl = true
            };

            #region Configurar Mail

            var mail = new MailMessage
            {
                From = new MailAddress(MailAccount, MailDisplayName)
            };

            foreach (var item in userNames) {
                mail.To.Add(new MailAddress(item.ToString()));
            }

            #endregion
            #region ImageMail
            var rootFolder = AppDomain.CurrentDomain.BaseDirectory;
            var logopath = Path.Combine(rootFolder, "MailImages/logo.jpg");
            var logo = new LinkedResource(logopath, MediaTypeNames.Image.Jpeg);
            logo.ContentId = "logo";
            var html = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);
            html.LinkedResources.Add(logo);
            #endregion

            #region Set values to mail

            mail.Subject = subject;
            mail.IsBodyHtml = true;
            mail.Body = body;
            mail.AlternateViews.Add(html);

            #endregion Set values to mail

            smtpClient.SendAsync(mail, null);
            smtpClient.SendCompleted += (sender, args) => {
                smtpClient.Dispose();
                mail.Dispose();
            };
        }

    }
}