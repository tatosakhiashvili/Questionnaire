using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Questionnaire.Models {
	public class InternationalPriceViewModel {
		public int? CompanyId { get; set; }
		public int? CountryId { get; set; }
		public List<InternationalPriceCountryViewModel> Countries { get; set; }
		public List<InternationalPriceCompanyViewModel> Companies { get; set; }
	}
}