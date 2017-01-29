using Questionnaire.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Domain;
using Questionnaire.Domain.Entities;

namespace Questionnaire.Repositories.DummyRepositories {
	public class InternationalPriceRepository : IInternationalPriceRepository {
		public IEnumerable<Company> GetCompanies(decimal userId, int languageId) {
			return new List<Company> {
				new Company { Name = "Name", Code = "1" }
			};
		}

		public IEnumerable<Country> GetCountries(decimal userId, int languageId) {
			return new List<Country> {
				new Country { Id = 1, Code = "1", NameEng = "Name Eng", NameGeo = "Name Geo" }
			};
		}

		public IEnumerable<InternationalTariff> GetInternationalTariffrs(string countryCode, string companyCode, decimal userId, int languageId) {
			return new List<InternationalTariff> {
				new InternationalTariff { Ind = "1", PriceC = "Price C", PriceRound = 1, SmsText = "Sms Text ...", Units = 1, ZonIndName = "Zon Ind Name" },
				new InternationalTariff { Ind = "1", PriceC = "Price C", PriceRound = 1, SmsText = "Sms Text ... 1", Units = 1, ZonIndName = "Zon Ind Name" }
			};
		}
	}
}
