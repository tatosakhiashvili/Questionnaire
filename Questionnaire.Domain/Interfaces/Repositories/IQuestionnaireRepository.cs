using Questionnaire.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Domain.Interfaces.Repositories {
	public interface IQuestionnaireRepository {
		List<Tree> GetFaqTree(DateTime date, bool onlyPublished, int languageId, decimal userId);
		IEnumerable<Tree> GetTreeForLink(int treeId, decimal userId, int languageId);
		List<Tree> GetFaqTreeForChange(int treeId, int languageId, decimal userId);
		List<Tree> GetFaqTreeForCopy(int treeId, int languageId, decimal userId);
		List<Tree> SearchInTree(int searchType, string searchTerm, string searchComment, bool onlyPublished, DateTime date, DateTime periodStart, DateTime periodEnd, int statusId, int ownerId, decimal userId, int languageId);
		IEnumerable<Sms> GetTreeSms(int treeId, DateTime date, int langId, decimal userId);
		Answer GetTreeAnswer(int treeId, DateTime date, int langId, decimal userId);
		string GetTreeAnswerName(int treeId, DateTime date, int langId, decimal userId);
		IEnumerable<Faq> GetFaqTreeFavourite(int languageId, decimal userId);
		IEnumerable<Faq> GetFaqTreeTop10(int languageId, decimal userId);
		IEnumerable<Faq> GetFaqTreeTop10Month(int languageId, decimal userId);
		IEnumerable<Faq> GetFaqTreeTop10Today(int languageId, decimal userId);
		IEnumerable<Faq> GetFaqTreeTopCustomerQuestions(string msisdn, int languageId, decimal userId);

		void AddSmsToTree(int treeId, int? groupId, string groupName, string sms, string comment, int position, decimal userId, int languageId);
		void RemoveSmsFromTree(int treeId, int? groupId, string groupName, string sms, string comment, decimal userId, int languageId);

		decimal AddTreeNode(int nodeId, string text, int statusId, DateTime fromDate, DateTime toDate, int newsDays, string comment, decimal userId);
		bool ChangeTreeNode(int nodeId, int parentId, string text, int statusId, DateTime fromDate, DateTime toDate, int newsDays, string comment, decimal userId);
		bool CopyTreeNode(int nodeId, int copyToTreeId, bool copyWithChildren, string text, int statusId, DateTime fromDate, DateTime toDate, int newsDays, string comment, decimal userId);
		bool MoveTreeNode(int nodeId, int copyToTreeId, string text, int statusId, DateTime fromDate, DateTime toDate, int newsDays, string comment, decimal userId);

		bool MoveNodeUp(int nodeId, int parentId, decimal userId, string comment);
		bool MoveNodeDown(int nodeId, int parentId, decimal userId, string comment);
		bool MoveInPreActive(int nodeId, int parentId, decimal userId, string comment);

		bool AddNodeToFavourite(int nodeId, decimal userId, string comment);
		bool RemoveNodeFromFavourite(int recordId, int nodeId, decimal userId, string comment);
		bool RemoveTreeNode(int nodeId, decimal userId);

		bool AddEmailAnswer(int nodeId, int answerId, DateTime fromdate, DateTime toDate, string answer, string htmlName, List<string> searchTags, string comment, bool saveAsTemplate, string templateName, decimal userId, int languageId);
		IEnumerable<AnswerTemplate> GetTreeEmailTemlpates(int treeId);

		IEnumerable<Owner> GetOwners(decimal userId, int languageId);
		IEnumerable<AnswerVersion> GetAnswerVersions(int nodeId, decimal userId, int languageId);

		IEnumerable<DeletedTree> GetFaqTreeDeleted(decimal userId, int languageId);

		bool RecoverTreeFromDeletedRecords(int nodeId, string comment, decimal userId, int languageId);

		IEnumerable<Reminder> GetReminderItems(decimal userId, int languageId);
		IEnumerable<string> GetEmailGroups(string number, decimal userId, int languageId);

		bool RegisterMsisdn(int id, int treeId, string number, string comments, decimal userId, int languageId);

		void SendNotificationLog(int type, int treeId, string msisdn, string email, string sms, decimal userId, int languageId);

		IEnumerable<EmailComparisonFile> GetEmailComparisonFiles(int treeId, int answerId, DateTime date, decimal userId, int languageId);
		void SaveTreeFile(int id, DateTime fromDate, DateTime toDate, int treeId, int answerId, string fileName, string comment, decimal userId, int languageId);
		void SaveTreeModalFile(int id, DateTime fromDate, DateTime toDate, int treeId, int answerId, string fileName, string comment, decimal userId, int languageId);

		void DeleteTreeFile(int id, DateTime fromDate, string fileName, decimal userId, int languageId);

		IEnumerable<GroupEmail> GetGroupEmails(decimal userId, int languageId);
		void SendGroupEmail(int treeId, string text, int sendHour, string email, decimal userId, int languageId);
		string GetCurrentCallMobileNumber(decimal userId, int languageId);
		IEnumerable<TreeStatus> GetStatuses();

		#region Daemon Actions

		IEnumerable<DaemonMessage> GetMessagesToSend();
		void AddMessageSend(int messageId);

		#endregion
	}
}