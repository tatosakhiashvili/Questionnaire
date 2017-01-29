using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Domain {
	public class Operator {
		public int Id { get; set; }
		public string Name { get; set; }
		public string NetworkCode { get; set; }
		public string NetworkType { get; set; }
		public string Display { get; set; }
		public string CustomerCare { get; set; }
		public string GprsRoamingStatus { get; set; }
	}
}
