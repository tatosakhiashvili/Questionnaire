using Questionnaire.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Domain;
using Questionnaire.Domain.Interfaces.Repositories;

namespace Questionnaire.Services {
	public class FaqService : IFaqService {
		private IQuestionnaireRepository _questionnaireRepository;
		public FaqService(IQuestionnaireRepository questionnaireRepository) {
			_questionnaireRepository = questionnaireRepository;
		}

		public IEnumerable<Faq> GetCustomerFaq(string msisdn) {
			return _questionnaireRepository.GetFaqTreeTopCustomerQuestions(msisdn, QuestionnaireContext.Current.LanguageId, QuestionnaireContext.Current.UserId);
		}

		public IEnumerable<Faq> GetFavouriteFaq() {
			return _questionnaireRepository.GetFaqTreeFavourite(QuestionnaireContext.Current.LanguageId, QuestionnaireContext.Current.UserId);
		}

		public IEnumerable<Faq> GetTopFaq(TopFaqEnumeration topType) {
			switch(topType) {
				case TopFaqEnumeration.TopTen:
					return _questionnaireRepository.GetFaqTreeTop10(QuestionnaireContext.Current.LanguageId, QuestionnaireContext.Current.UserId);
				case TopFaqEnumeration.TodayTopTen:
					return _questionnaireRepository.GetFaqTreeTop10Today(QuestionnaireContext.Current.LanguageId, QuestionnaireContext.Current.UserId);
				case TopFaqEnumeration.MonthTopTen:
					return _questionnaireRepository.GetFaqTreeTop10Month(QuestionnaireContext.Current.LanguageId, QuestionnaireContext.Current.UserId);
				default:
					return new List<Faq> { };
			}
		}
	}
}
