using Questionnaire.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Domain.Interfaces.Repositories {
	public interface IInternationalPriceRepository {
		IEnumerable<Company> GetCompanies(decimal userId, int languageId);
		IEnumerable<Country> GetCountries(decimal userId, int languageId);
		IEnumerable<InternationalTariff> GetInternationalTariffrs(string countryCode, string companyCode, decimal userId, int languageId);
	}
}
