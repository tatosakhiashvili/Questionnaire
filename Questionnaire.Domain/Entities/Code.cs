using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Domain.Entities {
	public class Code {
		public string Cc { get; set; }
		public string NameGeo { get; set; }
		public string NameEng { get; set; }
		public string TelCode { get; set; }
		public string CityNameGeo { get; set; }
		public string CityNameEng { get; set; }
		public string SmsText { get; set; }
		public string SmsTextCity { get; set; }
	}
}
