using Questionnaire.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Domain.Interfaces.Services {
	public interface IChatService {
		int AddInternalMessage(string text, int typeId, int priorityId, string comment, DateTime fromDate, DateTime toDate, DateTime processDate);
		bool ChangeInternalMessage(int id, string text, int typeId, int priorityId, string comment, DateTime fromDate, DateTime toDate, DateTime processDate);
		bool ExecuteInternalMessage(int id, string comment);
		bool DeleteInternalMessage(int id, string comment);
		InternalMessage GetInternalMessage(int id);
		IEnumerable<InternalMessage> GetInternalMessages();
		IEnumerable<InternalMessageFile> GetInternalMessageFiles(int id);
		IEnumerable<InternalMessageStatus> GetInternalMessageStatuses(int id);
		IEnumerable<InternalMessageType> GetInternalMessageTypes();
		IEnumerable<InternalMessagePriority> GetInternalMessagePriorities();
		void SaveFile(int messageId, string fileName);
		void RemoveFile(int messageId, string fileName);
	}
}
