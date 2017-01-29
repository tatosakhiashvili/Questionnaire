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
	public class InternationalCodeService : IInternationalCodeService {

		private IInternationalCodeRepository _internationalCodeRepository;

		public InternationalCodeService(IInternationalCodeRepository internationalCodeRepository) {
			_internationalCodeRepository = internationalCodeRepository;
		}

		public IEnumerable<Code> GetInternationalCodes() {
			return _internationalCodeRepository.GetInternationalCodes(QuestionnaireContext.Current.UserId, QuestionnaireContext.Current.LanguageId);
		}
	}
}
