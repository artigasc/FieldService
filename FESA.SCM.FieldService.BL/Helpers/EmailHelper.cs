using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FESA.SCM.FieldService.BL.Helpers {
	public class EmailHelper {

		private string MailAccount { get; set; }
		private string MailPassword { get; set; }
		private string SMTP { get; set; }
		private string MailDisplayName { get; set; }
		private string SystemUrl { get; set; }

		public EmailHelper(string mailAccount, string mailPassword, string sMTP, string mailDisplayName) {
			//MailAccount = "colaborador@cmscloud.pe";
			//MailPassword = "Cms@Cloud2016";
			//SMTP = "smtp.office365.com";
			//MailDisplayName = "Fereyros";


			MailAccount = "serviciodecampomovil@ferreyros.com.pe";
			MailPassword = "Enviosmtp2015";
			SMTP = "smtp.gmail.com";
			MailDisplayName = "Servicio de Campo Móvil";

			//MailAccount = mailAccount; // "colaborador@cmscloud.pe";
			//MailPassword = mailPassword; // "Cms@Cloud2016";
			//SMTP = sMTP; // "smtp.office365.com";
			//MailDisplayName = mailDisplayName; // "Pandero";

			//< add key = "mailAccount" value = "serviciodecampomovil@ferreyros.com.pe" />
			//< add key = "mailPassword" value = "Enviosmtp2015" />
			//< add key = "smtp" value = "smtp.gmail.com" />
			//< add key = "mailDisplayName" value = "Servicio de Campo Móvil" />


		}

		public async Task PostSendEmail(List<string> userNames, List<string> usersCC, string subject, string body, bool isHtml, List<Attachment> attachments) {
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

			if (userNames != null && userNames.Count > 0) {
				foreach (var item in userNames) {
					mail.To.Add(new MailAddress(item.ToString()));
				}
			}

			if (usersCC != null && usersCC.Count > 0) {
				foreach (var item in usersCC) {
					mail.CC.Add(item.ToString());
				}
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
