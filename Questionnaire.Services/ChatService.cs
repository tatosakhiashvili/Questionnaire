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
	public class ChatService : IChatService {
		private IChatRepository _chatRepository;

		public ChatService(IChatRepository chatRepository) {
			_chatRepository = chatRepository;
		}

		public int AddInternalMessage(string text, int typeId, int priorityId, string comment, DateTime fromDate, DateTime toDate, DateTime processDate) {
			return _chatRepository.AddInternalMessage(text, typeId, priorityId, comment, fromDate, toDate, processDate, QuestionnaireContext.Current.UserId, QuestionnaireContext.Current.LanguageId);
		}

		public bool ChangeInternalMessage(int id, string text, int typeId, int priorityId, string comment, DateTime fromDate, DateTime toDate, DateTime processDate) {
			return _chatRepository.ChangeInternalMessage(id, text, typeId, priorityId, comment, fromDate, toDate, processDate, QuestionnaireContext.Current.UserId, QuestionnaireContext.Current.LanguageId);
		}

		public bool DeleteInternalMessage(int id, string comment) {
			return _chatRepository.DeleteInternalMessage(id, comment, QuestionnaireContext.Current.UserId, QuestionnaireContext.Current.LanguageId);
		}

		public bool ExecuteInternalMessage(int id, string comment) {
			return _chatRepository.ExecuteInternalMessage(id, comment, QuestionnaireContext.Current.UserId, QuestionnaireContext.Current.LanguageId);
		}

		public InternalMessage GetInternalMessage(int id) {
			return _chatRepository.GetInternalMessage(id, QuestionnaireContext.Current.UserId, QuestionnaireContext.Current.LanguageId);
		}

		public IEnumerable<InternalMessageFile> GetInternalMessageFiles(int id) {
			return _chatRepository.GetInternalMessageFiles(id, QuestionnaireContext.Current.UserId, QuestionnaireContext.Current.LanguageId);
		}

		public IEnumerable<InternalMessagePriority> GetInternalMessagePriorities() {
			return _chatRepository.GetInternalMessagePriorities(QuestionnaireContext.Current.UserId, QuestionnaireContext.Current.LanguageId);
		}

		public IEnumerable<InternalMessage> GetInternalMessages() {
			return _chatRepository.GetInternalMessages(QuestionnaireContext.Current.UserId, QuestionnaireContext.Current.LanguageId);
		}

		public IEnumerable<InternalMessageStatus> GetInternalMessageStatuses(int id) {
			return _chatRepository.GetInternalMessageStatuses(id, QuestionnaireContext.Current.UserId, QuestionnaireContext.Current.LanguageId);
		}

		public IEnumerable<InternalMessageType> GetInternalMessageTypes() {
			return _chatRepository.GetInternalMessageTypes(QuestionnaireContext.Current.UserId, QuestionnaireContext.Current.LanguageId);
		}

		public void RemoveFile(int messageId, string fileName) {
			_chatRepository.RemoveFile(messageId, fileName, QuestionnaireContext.Current.UserId, QuestionnaireContext.Current.LanguageId);
		}

		public void SaveFile(int messageId, string fileName) {
			_chatRepository.SaveFile(messageId, fileName, QuestionnaireContext.Current.UserId, QuestionnaireContext.Current.LanguageId);
		}
	}
}
