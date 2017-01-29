using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Domain.Interfaces.Services {
	public interface IMailService {
		bool Send(string fromEmail, string toEmail, string title, string body, string attachementPath = "");
	}
}
