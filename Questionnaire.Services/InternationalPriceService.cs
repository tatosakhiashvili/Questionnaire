using Questionnaire.Domain.Interfaces.Repositories;
using Questionnaire.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Domain.Entities;
using Questionnaire.Domain;

namespace Questionnaire.Services {
	public class InternationalPriceService : IInternationalPriceService {

		private IInternationalPriceRepository _internationalPriceRepository;

		public InternationalPriceService(IInternationalPriceRepository internationalPriceRepository) {
			_internationalPriceRepository = internationalPriceRepository;
		}

		public IEnumerable<Company> GetCompanies() {
			return _internationalPriceRepository.GetCompanies(QuestionnaireContext.Current.UserId, QuestionnaireContext.Current.LanguageId);
		}

		public IEnumerable<Country> GetCountries() {
			return _internationalPriceRepository.GetCountries(QuestionnaireContext.Current.UserId, QuestionnaireContext.Current.LanguageId);
		}

		public IEnumerable<InternationalTariff> GetInternationalTariffrs(string countryCode, string companyCode) {
			return _internationalPriceRepository.GetInternationalTariffrs(countryCode, companyCode, QuestionnaireContext.Current.UserId, QuestionnaireContext.Current.LanguageId);
		}
	}
}
