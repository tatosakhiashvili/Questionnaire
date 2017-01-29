using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Domain {
	public class Answer {
		public int Id { get; set; }
		public int AnswerId { get; set; }
		public string FileName { get; set; }
		public DateTime FromDate { get; set; }
		public DateTime ToDate { get; set; }
		public string Comment { get; set; }
		public string Body { get; set; }
		public string FilePath { get; set; }
	}
}
