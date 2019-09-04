using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace FESA.SCM.ServiceGateway.Helper {

	public class EmailHelper {

		private string MailAccount { get; set; }
		private string MailPassword { get; set; }
		private string SMTP { get; set; }
		private string MailDisplayName { get; set; }
		private string SystemUrl { get; set; }

		public EmailHelper(string mailAccount, string mailPassword, string sMTP, string mailDisplayName) {
			MailAccount = "colaborador@cmscloud.pe";
			MailPassword = "Cms@Cloud2016";
			SMTP = "smtp.office365.com";
			MailDisplayName = "Fereyros";

			//MailAccount = mailAccount; // "colaborador@cmscloud.pe";
			//MailPassword = mailPassword; // "Cms@Cloud2016";
			//SMTP = sMTP; // "smtp.office365.com";
			//MailDisplayName = mailDisplayName; // "Pandero";
		}

		public async Task PostSendEmail(List<string> userNames, string subject, string body, bool isHtml, List<Attachment> attachments) {
			var smtpClient = new SmtpClient {
				Host = SMTP,
				Port = 587,
				UseDefaultCredentials = false,
				DeliveryMethod = SmtpDeliveryMethod.Network,
				Credentials = new NetworkCredential(MailAccount, MailPassword),
				EnableSsl = true
			};

			#region Configurar Mail

			var mail = new MailMessage {
				From = new MailAddress(MailAccount, MailDisplayName)
			};

			foreach (var item in userNames) {
				mail.To.Add(new MailAddress(item.ToString()));
			}

			#endregion

			if (attachments != null) {
				foreach (var item in attachments) {
					mail.Attachments.Add(item);
				}
			}

			//#region ImageMail
			//var rootFolder = AppDomain.CurrentDomain.BaseDirectory;
			//var logopath = Path.Combine(rootFolder, "MailImages/logo.jpg");
			//var logo = new LinkedResource(logopath, MediaTypeNames.Image.Jpeg);
			//logo.ContentId = "logo";
			//var html = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);
			//html.LinkedResources.Add(logo);
			//#endregion

			#region Set values to mail

			mail.Subject = subject;
			mail.IsBodyHtml = isHtml;
			mail.Body = body;
			//mail.AlternateViews.Add(html);

			#endregion Set values to mail

			//smtpClient.SendAsync(mail, null);
			await smtpClient.SendMailAsync(mail);// mail, null);
			smtpClient.SendCompleted += (sender, args) => {
				smtpClient.Dispose();
				mail.Dispose();
			};
			// smtpClient.SendCompleted += (sender, args) => {
			//    smtpClient.Dispose();
			//    mail.Dispose();
			//};
		}

	}



}