using Questionnaire.Domain.Interfaces.Repositories;
using Questionnaire.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Domain;

namespace Questionnaire.Services {
	public class RoamingService : IRoamingService {

		private IRoamingRepository _roamingRepository;
		private IQuestionnaireService _questionnaireService;
		private IMailService _mailService;

		public RoamingService(IQuestionnaireService questionnaireService, IRoamingRepository roamingRepository, IMailService mailService) {
			_roamingRepository = roamingRepository;
			_questionnaireService = questionnaireService;
			_mailService = mailService;
		}

		public IEnumerable<Country> GetCountries() {
			return _roamingRepository.GetCountries(QuestionnaireContext.Current.UserId, QuestionnaireContext.Current.LanguageId);
		}

		public IEnumerable<Operator> GetOperators(int countryId) {
			return _roamingRepository.GetOperators(countryId, QuestionnaireContext.Current.UserId, QuestionnaireContext.Current.LanguageId);
		}

		public IEnumerable<Tariff> GetTariffs(int operatorId) {
			return _roamingRepository.GetTariffs(operatorId, QuestionnaireContext.Current.UserId, QuestionnaireContext.Current.LanguageId);
		}

		public bool SendRoamingTariffNotification(string to, string title, string body) {
			var sendNotificationResponse = _mailService.Send(QuestionnaireContext.Current.Settings.Questionnaire.FromEmail, to, title, body);
			if(sendNotificationResponse) {
				_questionnaireService.SendNotificationLog(2, 0, string.Empty, to, body);
			}
			return sendNotificationResponse;
		}
	}
}
