using Questionnaire.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Domain.Interfaces.Repositories {
	public interface IChatRepository {
		int AddInternalMessage(string text, int typeId, int priorityId, string comment, DateTime fromDate, DateTime toDate, DateTime processDate, decimal userId, int languageId);
		bool ChangeInternalMessage(int id, string text, int typeId, int priorityId, string comment, DateTime fromDate, DateTime toDate, DateTime processDate, decimal userId, int languageId);
		bool ExecuteInternalMessage(int id, string comment, decimal userId, int languageId);
		bool DeleteInternalMessage(int id, string comment, decimal userId, int languageId);
		InternalMessage GetInternalMessage(int id, decimal userId, int languageId);
		IEnumerable<InternalMessage> GetInternalMessages(decimal userId, int languageId);
		IEnumerable<InternalMessageFile> GetInternalMessageFiles(int id, decimal userId, int languageId);
		IEnumerable<InternalMessageStatus> GetInternalMessageStatuses(int id, decimal userId, int languageId);
		IEnumerable<InternalMessageType> GetInternalMessageTypes(decimal userId, int languageId);
		IEnumerable<InternalMessagePriority> GetInternalMessagePriorities(decimal userId, int languageId);
		void SaveFile(int messageId, string fileName, decimal userId, int languageId);
		void RemoveFile(int messageId, string fileName, decimal userId, int languageId);
	}
}
