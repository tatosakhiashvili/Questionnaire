using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Models {
	public class EmailAnswerControlViewModel {
		public string EmailBody { get; set; }
		public string Email { get; set; }
		public List<string> Emails { get; set; }
	}
}
