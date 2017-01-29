using Questionnaire.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Questionnaire.Models {
	public class InternationalPriceCompanyViewModel {
		public string Name { get; set; }
		public string Code { get; set; }

		public static implicit operator InternationalPriceCompanyViewModel(Company company) {
			return new InternationalPriceCompanyViewModel {
				Code = company.Code,
				Name = company.Name
			};
		}
	}
}