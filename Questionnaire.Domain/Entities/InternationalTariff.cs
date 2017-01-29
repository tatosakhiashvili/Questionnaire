using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Domain {
	public class InternationalTariff {
		public string ZonIndName { get; set; }
		public string Ind { get; set; }
		public decimal PriceRound { get; set; }
		public decimal Units { get; set; }
		public string PriceC { get; set; }
		public string SmsText { get; set; }
	}
}
