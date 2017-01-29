using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Questionnaire.Repositories;
using Questionnaire.Domain.Interfaces.Services;
using Questionnaire.Domain.Interfaces.Repositories;
using System.Linq;

namespace Questionnaire.Test {
	[TestClass]
	public class QuestionnaireRepositoryTests : TestBase {

		private IQuestionnaireRepository _questionnaireRepository;
		private IQuestionnaireService _questionnaireService;

		public QuestionnaireRepositoryTests() {
			_questionnaireRepository = Resolve<IQuestionnaireRepository>();
			_questionnaireService = Resolve<IQuestionnaireService>();
		}

		[TestMethod]
		public void GetTreeTest() {
			//var tree = _questionnaireRepository.GetFaqTree(DateTime.Now, true, "en", 1);

			//_questionnaireRepository.GetTreeSms(25, DateTime.Now);

			var sss = _questionnaireRepository.GetFaqTreeTop10Today(1, 183).ToList();

			//var sms = _questionnaireService.GetTreeSms(25, DateTime.Now);

			//Assert.IsNotNull(tree);
		}
	}
}
