using Questionnaire.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Questionnaire.Models {
	public class InternationalPriceCountryViewModel {
		public string NameGeo { get; set; }
		public string NameEng { get; set; }
		public string Code { get; set; }

		public static implicit operator InternationalPriceCountryViewModel(Country country) {
			return new InternationalPriceCountryViewModel {
				Code = country.Code,
				NameGeo = country.NameGeo,
				NameEng = country.NameEng
			};
		}
	}
}