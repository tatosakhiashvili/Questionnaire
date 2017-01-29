using Questionnaire.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Questionnaire.Models {
	public class RoamingViewModel {
		public List<RoamingCountryViewModel> Countries { get; set; }
	}

	public class RoamingCountryViewModel {
		public int Id { get; set; }
		public string NameGeo { get; set; }
		public string NameEng { get; set; }
		public string Code { get; set; }

		public static implicit operator RoamingCountryViewModel(Country country) {
			return new RoamingCountryViewModel {
				Id = country.Id,
				NameGeo = country.NameGeo,
				NameEng = country.NameEng,
				Code = country.Code
			};
		}
	}
}