using Questionnaire.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Services {
	public class MailService : IMailService {

		public MailService() {

		}

		public bool Send(string fromEmail, string toEmail, string title, string body, string attachementPath = "") {
			try {
				MailMessage mail = new MailMessage(fromEmail, toEmail);
				SmtpClient client = new SmtpClient();
				client.Port = 25;
				client.DeliveryMethod = SmtpDeliveryMethod.Network;
				client.UseDefaultCredentials = false;
				client.Host = "mail.geocell.com.ge";
				mail.Subject = title;
				mail.IsBodyHtml = true;
				mail.Body = body;

				if(!string.IsNullOrEmpty(attachementPath)) {
					Attachment attachment = new Attachment(attachementPath);
					mail.Attachments.Add(attachment);
				}

				client.Send(mail);
				return true;
			} catch(Exception) {
				return false;
			}
		}
	}
}
