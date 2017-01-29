using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Domain.Entities {
	public class Reminder {
		public int Id { get; set; }
		public string Caption { get; set; }
		public DateTime FromDate { get; set; }
		public DateTime ToDate { get; set; }
		public DateTime InDate { get; set; }
		public string User { get; set; }
		public string Comments { get; set; }
		public decimal Remind { get; set; }
	}
}
