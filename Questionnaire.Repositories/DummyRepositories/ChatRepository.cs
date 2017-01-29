using Questionnaire.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Domain.Entities;
using Questionnaire.Domain;

namespace Questionnaire.Repositories.DummyRepositories {
	public class ChatRepository : IChatRepository {
		public ChatRepository() {

		}

		public int AddInternalMessage(string text, int typeId, int priorityId, string comment, DateTime fromDate, DateTime toDate, DateTime processDate, decimal userId, int languageId) {
			throw new NotImplementedException();
		}

		public bool ChangeInternalMessage(int id, string text, int typeId, int priorityId, string comment, DateTime fromDate, DateTime toDate, DateTime processDate, decimal userId, int languageId) {
			throw new NotImplementedException();
		}

		public bool DeleteInternalMessage(int id, string comment, decimal userId, int languageId) {
			throw new NotImplementedException();
		}

		public bool ExecuteInternalMessage(int id, string comment, decimal userId, int languageId) {
			throw new NotImplementedException();
		}

		public InternalMessage GetInternalMessage(int id, decimal userId, int languageId) {
			return new InternalMessage {
				Id = 1,
				Comment = "Comment",
				CreateDate = DateTime.Now,
				LastUpdateDate = DateTime.Now,
				Operator = "Operator",
				Owner = "Owner",
				Priority = "Priority",
				PriorityId = 1,
				ProcessDate = DateTime.Now,
				Status = "Status",
				Text = "Text",
				TypeId = 1
			};
		}

		public IEnumerable<InternalMessageFile> GetInternalMessageFiles(int id, decimal userId, int languageId) {
			return new List<InternalMessageFile> {
				 new InternalMessageFile { Id = 1, Name = "Name", CreateDate = DateTime.Now, Operator = "Operator", MessageId = 1 }
			};
		}

		public IEnumerable<InternalMessagePriority> GetInternalMessagePriorities(decimal userId, int languageId) {
			return new List<InternalMessagePriority> {
				new InternalMessagePriority { Id = 1, Name = "Hight" },
				new InternalMessagePriority { Id = 2, Name = "Medium" },
				new InternalMessagePriority { Id = 3, Name = "Low" }
			};
		}

		public IEnumerable<InternalMessage> GetInternalMessages(decimal userId, int languageId) {
			var internalMessage = new InternalMessage {
				Id = 1,
				Comment = "Comment",
				CreateDate = DateTime.Now,
				LastUpdateDate = DateTime.Now,
				ProcessDate = DateTime.Now,
				Operator = "Operator",
				Owner = "Owner",
				Priority = "Priority",
				Status = "Status",
				Text = "Text"
			};

			return new List<InternalMessage> {
				internalMessage, internalMessage, internalMessage, internalMessage
			};
		}

		public IEnumerable<InternalMessageStatus> GetInternalMessageStatuses(int id, decimal userId, int languageId) {
			var status = new InternalMessageStatus {
				Id = 1,
				Name = "Name",
				Operator = "Operator",
				CreateDate = DateTime.Now
			};

			return new List<InternalMessageStatus> {
				status, status, status, status, status, status
			};
		}

		public IEnumerable<InternalMessageType> GetInternalMessageTypes(decimal userId, int languageId) {
			return new List<InternalMessageType> {
				new InternalMessageType { Id = 1, Name = "მოთხოვნა" },
				new InternalMessageType { Id = 2, Name = "სატესტო" },
				new InternalMessageType { Id = 3, Name = "ჩვეულებრივი" }
			};
		}

		public void RemoveFile(int messageId, string fileName, decimal userId, int languageId) {
			throw new NotImplementedException();
		}

		public void SaveFile(int messageId, string fileName, decimal userId, int languageId) {
			throw new NotImplementedException();
		}
	}
}
