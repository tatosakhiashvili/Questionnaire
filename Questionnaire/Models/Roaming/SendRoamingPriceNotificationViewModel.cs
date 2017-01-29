using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Models {
	public class SendRoamingPriceNotificationViewModel {
		public string Email { get; set; }
		public string EmailSubject { get; set; }
		public int OperatorId { get; set; }
	}
}
