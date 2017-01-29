using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Domain {
	public class Sms {
		public int Id { get; set; }
		public string GroupName { get; set; }
		public int? SortOrder { get; set; }
		public string Comment { get; set; }
		public string Text { get; set; }
	}
}
