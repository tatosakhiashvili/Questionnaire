using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Domain {
	public class Country {
		public int Id { get; set; }
		public string NameGeo { get; set; }
		public string NameEng { get; set; }
		public string Code { get; set; }
	}
}
