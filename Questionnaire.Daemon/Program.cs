using Questionnaire.Configuration;
using Questionnaire.Domain;
using Questionnaire.Domain.Interfaces.Repositories;
using Questionnaire.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Questionnaire.Daemon {
	class Program {
		static void Main(string[] args) {
			var _resolver = new UnityResolver { };
			var _settings = _resolver.Resolve<Settings>();

			//InitializeContext(_resolver);

			//var sss = QuestionnaireContext.Current.IsAuthenticated;



			Timer t = new Timer(CallBack, null, TimeSpan.FromSeconds(0), TimeSpan.FromMinutes(_settings.Questionnaire.DaemonRunInterval));
			Console.ReadLine();
		}

		private static void CallBack(object o) {
			Console.WriteLine("Checking messages to send {0}", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));

			var _resolver = new UnityResolver { };
			var _questionnaireRepository = _resolver.Resolve<IQuestionnaireRepository>();
			var _mailService = _resolver.Resolve<IMailService>();
			var _settings = _resolver.Resolve<Settings>();

			var messagesToSend = _questionnaireRepository.GetMessagesToSend();

			if(messagesToSend.Count() == 0) {
				Console.WriteLine("No messages to send");
			} else {
				Console.WriteLine("Got {0} messages to send", messagesToSend.Count());
				Console.WriteLine("Start sending message {0}", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));

				foreach(var message in messagesToSend) {
					var sendResponse = _mailService.Send(_settings.Questionnaire.FromEmail, message.Receiver, message.Subject, message.MessageText);
					if(sendResponse) {
						_questionnaireRepository.AddMessageSend(message.Id);
						Console.WriteLine("Message has been sent successfully to {0}", message.Receiver);
					} else {
						Console.WriteLine("Couldn't send message to {0}", message.Receiver);
					}
				}

				Console.WriteLine("End sending message {0}", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
			}
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine();
			GC.Collect();
		}
	}
}
