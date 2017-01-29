using Questionnaire.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Models {
	public class EmailNotificationViewModel {
		public string Body { get; set; }

		public static implicit operator EmailNotificationViewModel(Answer answer) {
			return new EmailNotificationViewModel {
				Body = answer.Body
			};
		}
	}
}
