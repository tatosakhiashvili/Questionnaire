using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Domain.Entities {
	public class AnswerVersion {
		public int Id { get; set; }
		public DateTime FromDate { get; set; }
		public DateTime ToDate { get; set; }
		public string FileName { get; set; }
		public string Username { get; set; }
		public string Comment { get; set; }
	}
}
