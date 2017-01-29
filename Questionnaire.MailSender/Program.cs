using Microsoft.Practices.Unity;
using Questionnaire.Configuration;
using Questionnaire.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Questionnaire.MailSender {
	class Program {
		static void Main(string[] args) {

			//var _config = ConfigurationManager.OpenExeConfiguration(string.Empty);

			//var sss = _config.GetSection("questionnaire") as QuestionnaireConfig;
			

			var r = new UnityResolver();
			var _questionnaireService = r.Resolve<IQuestionnaireService>();

			//string ssssss = "";
			////var sss = _questionnaireService.GetTree(DateTime.Now, true, false);

			////Timer t = new Timer(CallBack, null, TimeSpan.FromSeconds(0), TimeSpan.FromMinutes(5));
			////Console.ReadLine();
		}

		private static void CallBack(object o) {
			Console.WriteLine("Doing ... " + DateTime.Now);
			GC.Collect();
		}
	}
}
