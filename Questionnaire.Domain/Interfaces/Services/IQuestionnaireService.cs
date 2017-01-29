using Questionnaire.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Domain.Interfaces.Services {
	public interface IQuestionnaireService {
		IEnumerable<Tree> GetTree(DateTime date, bool onlyPublished, bool loadFromCache);
		IEnumerable<Tree> GetTreeForLink(int treeId);
		IEnumerable<Tree> GetTreeForChange(int nodeId);
		IEnumerable<Tree> GetTreeForCopy(int nodeId);
		IEnumerable<Tree> SearchInTree(int searchType, string searchTerm, string searchComment, bool onlyPublished, DateTime date, DateTime periodStart, DateTime periodEnd, int statusId, int ownerId);

		IEnumerable<Sms> GetTreeSms(int treeId, DateTime date);
		void AddSmsToTree(int treeId, int? groupId, string groupName, string groupPreviousName, int position, List<string> smsList, string comment);
		Answer GetTreeAnswer(int treeId, DateTime date);
		IEnumerable<AnswerTemplate> GetTreeEmailTemlpates(int treeId, bool loadTemplates = false);
		IEnumerable<AnswerTemplate> GetTreeEmailTemplatesByNames(int treeId, List<string> names);

		decimal AddTreeNode(int nodeId, string text, int statusId, DateTime fromDate, DateTime toDate, int newsDays, string comment);
		bool ChangeTreeNode(int nodeId, string text, int statusId, DateTime fromDate, DateTime toDate, int newsDays, string comment);
		bool CopyTreeNode(int nodeId, int copyToTreeId, bool copyWithChildre, string text, int statusId, DateTime fromDate, DateTime toDate, int newsDays, string comment);
		bool MoveTreeNode(int nodeId, int moveToTreeId, string text, int statusId, DateTime fromDate, DateTime toDate, int newsDays, string comment);
		bool MoveNodeUp(int nodeId);
		bool MoveNodeDown(int nodeId);
		bool MoveInPreActive(int nodeId);
		bool RemoveTreeNode(int nodeId);
		bool AddNodeToFavourite(int nodeId);
		bool RemoveNodeFromFavourite(int recordId, int nodeId);

		bool AddEmailAnswer(int nodeId, int answerId, DateTime fromDate, DateTime toDate, string answer, string pureAnswer, string comment, bool saveAsTemplate, string templateName);

		bool SendEmail(int nodeId, string email, string emailBody);
		bool SendSms(int treeId, string number, List<string> notifications);
		void SendNotificationLog(int type, int treeId, string msisdn, string email, string sms);

		IEnumerable<Owner> GetOwners();
		IEnumerable<AnswerVersion> GetAnswerVersions(int nodeId);

		IEnumerable<DeletedTree> GetTreeRemovedRecords();
		bool RecoverTreeFromDeletedRecords(int nodeId, string comment);

		IEnumerable<Reminder> GetReminderItems();
		IEnumerable<string> GetEmailGroups(string number);
		bool RegisterMsisdn(string number, int treeId);

		IEnumerable<EmailComparisonFile> GetEmailComparisonFiles(int treeId, int answerId, DateTime date);
		void SaveTreeFile(int id, DateTime fromDate, DateTime toDate, int treeId, int answerId, string fileName, string comment);
		void SaveTreeModalFile(int id, DateTime fromDate, DateTime toDate, int treeId, int answerId, string fileName, string comment);
		void RemoveTreeFile(int id, DateTime fromDate, string fileName);

		void SaveTreeNodeFile();
		void RemoveTreeNodeFile();

		IEnumerable<GroupEmail> GetGroupEmails();
		void SendGroupEmail(int treeId, string text, int sendHour, List<string> emails);

		string GetCurrentCallMobileNumber();
		IEnumerable<TreeStatus> GetStatuses();
	}
}
