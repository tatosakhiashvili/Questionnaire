using Questionnaire.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Questionnaire.Models {

	public class InternationalTariffsViewModel {
		public List<InternationalTariffViewModel> Records { get; set; }
	}

	public class InternationalTariffViewModel {
		public string ZonIndName { get; set; }
		public string Ind { get; set; }
		public decimal PriceRound { get; set; }
		public decimal Units { get; set; }
		public string PriceC { get; set; }
		public string SmsText { get; set; }

		public static implicit operator InternationalTariffViewModel(InternationalTariff tariff) {
			return new InternationalTariffViewModel {
				ZonIndName = tariff.ZonIndName,
				Ind = tariff.Ind,
				PriceRound = tariff.PriceRound,
				Units = tariff.Units,
				PriceC = tariff.PriceC,
				SmsText = tariff.SmsText
			};
		}
	}
}