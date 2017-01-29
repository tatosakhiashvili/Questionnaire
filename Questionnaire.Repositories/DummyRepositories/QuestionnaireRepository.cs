using Oracle.ManagedDataAccess.Client;
using Questionnaire.Domain;
using Questionnaire.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Domain.Entities;

namespace Questionnaire.Repositories.DummyRepositories {
	public class QuestionnaireRepository : IQuestionnaireRepository {
		private OracleContext _context;
		private ICacheRepository _cacheRepository;

		public QuestionnaireRepository(OracleContext context, ICacheRepository cacheRepository) {
			_context = context;
			_cacheRepository = cacheRepository;
		}

		public List<Tree> GetFaqTree(DateTime date, bool onlyPublished, int languageId, decimal userId) {
			var result = new List<Tree> { };
			result.Add(new Tree { Id = 1, ParentId = -1, Text = "კითხვარი' 1" });
			result.Add(new Tree { Id = 2, ParentId = -1, Text = "კითხვარი' 2" });
			result.Add(new Tree { Id = 3, ParentId = 1, Text = "კითხვარი 3" });
			result.Add(new Tree { Id = 4, ParentId = 1, Text = "კითხვარი 4" });
			result.Add(new Tree { Id = 5, ParentId = 1, Text = "კითხვარი 5" });
			result.Add(new Tree { Id = 6, ParentId = 2, Text = "კითხვარი 6", NewsDays = 5, FromDate = DateTime.Now, ToDate = DateTime.Now.AddDays(5) });
			result.Add(new Tree { Id = 7, ParentId = 2, Text = "კითხვარი 7" });
			result.Add(new Tree { Id = 8, ParentId = 2, Text = "კითხვარი 8" });
			result.Add(new Tree { Id = 9, ParentId = 3, Text = "კითხვარი 9" });
			result.Add(new Tree { Id = 10, ParentId = 9, Text = "კითხვარი 10" });
			return result;
		}

		public List<Tree> SearchInTree(int searchType, string searchTerm, string searchComment, bool onlyPublished, DateTime date, DateTime periodStart, DateTime periodEnd, int statusId, int ownerId, decimal userId, int languageId) {
			var result = new List<Tree> { };
			result.Add(new Tree { Id = 1, ParentId = -1, Text = "კითხვარი' 1" });
			return result;
		}

		public IEnumerable<Sms> GetTreeSms(int treeId, DateTime date, int langId, decimal userId) {
			return new List<Sms> { new Sms { Id = 1, Text = "Test Test Test Test", GroupName = "Group 1" } };
		}

		public IEnumerable<Tree> GetTreeForLink(int treeId, decimal userId, int languageId) {
			throw new NotImplementedException { };
		}

		public Answer GetTreeAnswer(int treeId, DateTime date, int langId, decimal userId) {
			var _body = @"
						<p>
							<a class='editor-internal-link' href='#' data-node-id='6'>[კითხვარი 6]</a>
								&nbsp;sd as dsa dsa dsa d asd as dsa d as das d&nbsp;
							<a href='http://google.com'>google.com</a>
							&nbsp;ada sdsa das das dsa dsa dsa dsad asd as d
						</p>
					";

			_body = string.Format(_body, treeId);

			return new Answer {
				Id = treeId,
				Body = _body
			};
		}

		public string GetTreeAnswerName(int treeId, DateTime date, int langId, decimal userId) {
			var answerName = "76b44501ac9d48228d7b4e20eb8acdaa";
			return answerName;
		}

		public IEnumerable<Faq> GetFaqTreeFavourite(int languageId, decimal userId) {
			return new List<Faq> { new Faq { Id = 1, Name = "Test 1" },
														new Faq { Id = 2, Name = "Test 2" },
														new Faq { Id = 3, Name = "Test 3" },
														new Faq { Id = 4, Name = "Test 4" },
														new Faq { Id = 5, Name = "Test 5" },
														new Faq { Id = 6, Name = "Test 6" },
														new Faq { Id = 7, Name = "Test 7" },
														new Faq { Id = 8, Name = "Test 8" },
														new Faq { Id = 9, Name = "Test 9" },
														new Faq { Id = 10, Name = "Test 10" }};
		}

		public IEnumerable<Faq> GetFaqTreeTop10(int languageId, decimal userId) {
			return new List<Faq> { new Faq { Id = 1, Name = "Test 1" },
															 new Faq { Id = 2, Name = "Test 2" },
															 new Faq { Id = 3, Name = "Test 3" },
															 new Faq { Id = 4, Name = "Test 4" },
															 new Faq { Id = 5, Name = "Test 5" },
															 new Faq { Id = 6, Name = "Test 6" },
															 new Faq { Id = 7, Name = "Test 7" },
															 new Faq { Id = 8, Name = "Test 8" },
															 new Faq { Id = 9, Name = "Test 9" },
															 new Faq { Id = 10, Name = "Test 10" } };
		}

		public IEnumerable<Faq> GetFaqTreeTop10Month(int languageId, decimal userId) {
			return new List<Faq> {
																		new Faq { Id = 1, Name = "Test 1" },
																		new Faq { Id = 2, Name = "Test 2" },
																		new Faq { Id = 3, Name = "Test 3" },
																		new Faq { Id = 4, Name = "Test 4" },
																		new Faq { Id = 5, Name = "Test 5" },
																		new Faq { Id = 6, Name = "Test 6" },
																		new Faq { Id = 7, Name = "Test 7" },
																		new Faq { Id = 8, Name = "Test 8" },
																		new Faq { Id = 9, Name = "Test 9" },
																		new Faq { Id = 10, Name = "Test 10" }
														};
		}

		public IEnumerable<Faq> GetFaqTreeTop10Today(int languageId, decimal userId) {
			return new List<Faq> {
															new Faq { Id = 1, Name = "Test 1" },
															new Faq { Id = 2, Name = "Test 2" },
															new Faq { Id = 3, Name = "Test 3" },
															new Faq { Id = 4, Name = "Test 4" },
															new Faq { Id = 5, Name = "Test 5" }
														};
		}

		public IEnumerable<Faq> GetFaqTreeTopCustomerQuestions(string msisdn, int languageId, decimal userId) {
			return new List<Faq> {
																		new Faq { Id = 1, Name = "Test 1" },
																		new Faq { Id = 2, Name = "Test 2" },
														};
		}

		public void AddSmsToTree(int treeId, int? groupId, string groupName, string sms, string comment, int position, decimal userId, int languageId) {

		}

		public void RemoveSmsFromTree(int treeId, int? groupId, string groupName, string sms, string comment, decimal userId, int languageId) {

		}

		public bool MoveNodeUp(int nodeId, int parentId, decimal userId, string comment) {
			return true;
		}

		public bool MoveNodeDown(int nodeId, int parentId, decimal userId, string comment) {
			return true;
		}

		public bool MoveInPreActive(int nodeId, int parentId, decimal userId, string comment) {
			return true;
		}

		public bool AddNodeToFavourite(int nodeId, decimal userId, string comment) {
			return true;
		}

		public bool RemoveNodeFromFavourite(int recordId, int nodeId, decimal userId, string comment) {
			return true;
		}

		public bool RemoveTreeNode(int nodeId, decimal userId) {
			return true;
		}

		public List<Tree> GetFaqTreeForChange(int treeId, int languageId, decimal userId) { //TODO: Implementor
			var result = _cacheRepository.Fetch(Tree.CacheKey + QuestionnaireContext.Current.UserId); //TODO: Store for temporary usage            
			if(result != null) {
				return result as List<Tree>;
			}
			return null;
		}

		public List<Tree> GetFaqTreeForCopy(int treeId, int languageId, decimal userId) { //TODO: Implementor
			var result = _cacheRepository.Fetch(Tree.CacheKey + QuestionnaireContext.Current.UserId); //TODO: Store for temporary usage            
			if(result != null) {
				return result as List<Tree>;
			}
			return null;
		}

		public decimal AddTreeNode(int nodeId, string text, int statusId, DateTime fromDate, DateTime toDate, int newsDays, string comment, decimal userId) {
			return 1;
		}

		public bool ChangeTreeNode(int nodeId, int parentId, string text, int statusId, DateTime fromDate, DateTime toDate, int newsDays, string comment, decimal userId) {
			return true;
		}

		public bool CopyTreeNode(int nodeId, int copyToTreeId, bool copyWithChildren, string text, int statusId, DateTime fromDate, DateTime toDate, int newsDays, string comment, decimal userId) {
			return true;
		}

		public bool MoveTreeNode(int nodeId, int copyToTreeId, string text, int statusId, DateTime fromDate, DateTime toDate, int newsDays, string comment, decimal userId) {
			return true;
		}

		public bool AddEmailAnswer(int nodeId, int answerId, DateTime fromdate, DateTime toDate, string answer, string htmlName, List<string> searchTags, string comment, bool saveAsTemplate, string templateName, decimal userId, int languageId) {
			return true;
		}

		public IEnumerable<AnswerTemplate> GetTreeEmailTemlpates(int treeId) {
			return new List<AnswerTemplate> {
				new AnswerTemplate { Id = 1, FileName = "8825ecf2d75d4d0aa935f28856d4f4e1", Name = "შაბლონი 1" },
				new AnswerTemplate { Id = 2, FileName = "8825ecf2d75d4d0aa935f28856d4f4e2", Name = "შაბლონი 2" },
				new AnswerTemplate { Id = 3, FileName = "8825ecf2d75d4d0aa935f28856d4f4e2", Name = "შაბლონი 3" },
				new AnswerTemplate { Id = 4, FileName = "8825ecf2d75d4d0aa935f28856d4f4e2", Name = "შაბლონი 4" },
				new AnswerTemplate { Id = 5, FileName = "8825ecf2d75d4d0aa935f28856d4f4e2", Name = "შაბლონი 5" },
				new AnswerTemplate { Id = 6, FileName = "8825ecf2d75d4d0aa935f28856d4f4e2", Name = "შაბლონი 6" },
				new AnswerTemplate { Id = 6, FileName = "8825ecf2d75d4d0aa935f28856d4f4e2", Name = "შაბლონი 7" },
			};
		}

		public IEnumerable<Owner> GetOwners(decimal userId, int languageId) {
			return new List<Owner> { new Owner { Id = 1, Name = "test" } };
		}

		public IEnumerable<AnswerVersion> GetAnswerVersions(int nodeId, decimal userId, int languageId) {
			return new List<AnswerVersion> {
				 new AnswerVersion { Id = 1, FileName = "0b709e09a68342738c152e8c3847f79b", FromDate = DateTime.Now, ToDate = DateTime.Now.AddDays(-1) },
				 new AnswerVersion { Id = 2, FileName = "2c071a7724594e5b9adb969c009720ea", FromDate = DateTime.Now, ToDate = DateTime.Now.AddDays(-1) },
			};
		}

		public IEnumerable<DeletedTree> GetFaqTreeDeleted(decimal userId, int languageId) {
			throw new NotImplementedException();
		}

		public bool RecoverTreeFromDeletedRecords(int nodeId, string comment, decimal userId, int languageId) {
			throw new NotImplementedException();
		}

		public IEnumerable<Reminder> GetReminderItems(decimal userId, int languageId) {
			throw new NotImplementedException();
		}

		public IEnumerable<string> GetEmailGroups(string number, decimal userId, int languageId) {
			throw new NotImplementedException();
		}

		public bool RegisterMsisdn(int id, int treeId, string number, string comments, decimal userId, int languageId) {
			throw new NotImplementedException();
		}

		public void SendNotificationLog(int type, int treeId, string msisdn, string email, string sms, decimal userId, int languageId) {
			throw new NotImplementedException();
		}

		public IEnumerable<EmailComparisonFile> GetEmailComparisonFiles(int treeId, int answerId, DateTime date, decimal userId, int languageId) {
			return new List<EmailComparisonFile> {

			};
		}

		public void SaveTreeFile(int id, DateTime fromDate, DateTime toDate, int treeId, int answerId, string fileName, string comment, decimal userId, int languageId) {
			throw new NotImplementedException();
		}

		public void SaveTreeModalFile(int id, DateTime fromDate, DateTime toDate, int treeId, int answerId, string fileName, string comment, decimal userId, int languageId) {
			throw new NotImplementedException();
		}

		public void DeleteTreeFile(int id, DateTime fromDate, string fileName, decimal userId, int languageId) {
			throw new NotImplementedException();
		}

		public IEnumerable<GroupEmail> GetGroupEmails(decimal userId, int languageId) {
			return new List<GroupEmail> {
				 new GroupEmail { Id = 1, Email = "jondi@gmail.com" },
				 new GroupEmail { Id = 2, Email = "jumberi@gmail.com" },
				 new GroupEmail { Id = 3, Email = "jambuli@gmail.com" },
			};
		}

		public void SendGroupEmail(int treeId, string text, int sendHour, string email, decimal userId, int languageId) {
			throw new NotImplementedException();
		}

		public string GetCurrentCallMobileNumber(decimal userId, int languageId) {
			return "554101232";
		}

		public IEnumerable<TreeStatus> GetStatuses() {
			throw new NotImplementedException();
		}

		public IEnumerable<DaemonMessage> GetMessagesToSend() {
			throw new NotImplementedException();
		}

		public void AddMessageSend(int messageId) {
			throw new NotImplementedException();
		}
	}
}