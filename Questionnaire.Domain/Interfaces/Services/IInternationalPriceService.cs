using Questionnaire.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Domain.Interfaces.Services {
	public interface IInternationalPriceService {
		IEnumerable<Company> GetCompanies();
		IEnumerable<Country> GetCountries();
		IEnumerable<InternationalTariff> GetInternationalTariffrs(string countryCode, string companyCode);
	}
}
