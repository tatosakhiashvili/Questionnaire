using Questionnaire.Domain;
using Questionnaire.Domain.Interfaces.Repositories;
using Questionnaire.Domain.Interfaces.Services;
using Questionnaire.Domain.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Domain.Entities;

namespace Questionnaire.Services {
	public class QuestionnaireService : IQuestionnaireService {
		private IQuestionnaireRepository _questionnaireRepository;
		private ICacheRepository _cacheRepository;
		private IMailService _mailService;

		public QuestionnaireService(IMailService mailService, ICacheRepository cacheRepository, IQuestionnaireRepository questionnaireRepository) {
			_questionnaireRepository = questionnaireRepository;
			_cacheRepository = cacheRepository;
			_mailService = mailService;
		}

		public void AddSmsToTree(int treeId, int? groupId, string groupName, string groupPreviousName, int position, List<string> smsList, string comment) {
			var userId = QuestionnaireContext.Current.UserId;
			var languageId = QuestionnaireContext.Current.LanguageId;

			if(!string.IsNullOrEmpty(groupPreviousName) && groupName != groupPreviousName) {
				_questionnaireRepository.RemoveSmsFromTree(treeId, groupId, groupPreviousName, null, comment, userId, languageId); //To remove old sms list
			}

			foreach(var sms in smsList) {
				_questionnaireRepository.AddSmsToTree(treeId, groupId, groupName, sms, comment, position, userId, languageId);
			}
		}

		public Answer GetTreeAnswer(int treeId, DateTime date) {
			//var treeAnswerName = _questionnaireRepository.GetTreeAnswerName(treeId, date, QuestionnaireContext.Current.LanguageId, QuestionnaireContext.Current.UserId);
			var treeAnswer = _questionnaireRepository.GetTreeAnswer(treeId, date, QuestionnaireContext.Current.LanguageId, QuestionnaireContext.Current.UserId);

			if(treeAnswer == null) {
				return new Answer {
					Id = treeId,
					AnswerId = -1,
					Body = string.Empty,
				};
			}

			var emailAnswerHtmlName = treeAnswer.FileName;
			var emailAnswerPack = UtilExtenders.MapPath("~/Uploads/FaqTree/EmailAnswers");
			var emailAnswerPath = Path.Combine(emailAnswerPack, emailAnswerHtmlName + ".htm");
			var htmlBody = string.Empty;

			if(File.Exists(emailAnswerPath)) {
				htmlBody = System.IO.File.ReadAllText(emailAnswerPath);
			}

			return new Answer {
				Id = treeId,
				AnswerId = treeAnswer.AnswerId,
				Body = htmlBody,
				FileName = treeAnswer.FileName,
				FromDate = treeAnswer.FromDate,
				ToDate = treeAnswer.ToDate,
				Comment = treeAnswer.Comment,
				FilePath = emailAnswerPath
			};
		}

		public decimal AddTreeNode(int nodeId, string text, int statusId, DateTime fromDate, DateTime toDate, int newsDays, string comment) {
			return _questionnaireRepository.AddTreeNode(nodeId, text, statusId, fromDate, toDate, newsDays, comment, QuestionnaireContext.Current.UserId);
		}

		public bool ChangeTreeNode(int nodeId, string text, int statusId, DateTime fromDate, DateTime toDate, int newsDays, string comment) {
			var cacheTree = GetTree(new DateTime(1900, 1, 1), true, false);
			if(cacheTree != null) {
				var nodeItem = (cacheTree as List<Tree>).FirstOrDefault(x => x.Id == nodeId);
				if(nodeItem != null) {
					var parentId = nodeItem.ParentId;
					return _questionnaireRepository.ChangeTreeNode(nodeId, parentId, text, statusId, fromDate, toDate, newsDays, comment, QuestionnaireContext.Current.UserId);
				}
			}
			return false;
		}

		public bool CopyTreeNode(int nodeId, int copyToTreeId, bool copyWithChildren, string text, int statusId, DateTime fromDate, DateTime toDate, int newsDays, string comment) {
			return _questionnaireRepository.CopyTreeNode(nodeId, copyToTreeId, copyWithChildren, text, statusId, fromDate, toDate, newsDays, comment, QuestionnaireContext.Current.UserId);
		}

		public bool MoveTreeNode(int nodeId, int moveToTreeId, string text, int statusId, DateTime fromDate, DateTime toDate, int newsDays, string comment) {
			return _questionnaireRepository.MoveTreeNode(nodeId, moveToTreeId, text, statusId, fromDate, toDate, newsDays, comment, QuestionnaireContext.Current.UserId);
		}

		public bool MoveNodeDown(int nodeId) {
			var cacheTree = _cacheRepository.Fetch(Tree.CacheKey + QuestionnaireContext.Current.UserId);
			if(cacheTree != null) {
				var nodeItem = (cacheTree as List<Tree>).FirstOrDefault(x => x.Id == nodeId);
				if(nodeItem != null) {
					var parentId = nodeItem.ParentId;
					return _questionnaireRepository.MoveNodeDown(nodeId, parentId, QuestionnaireContext.Current.UserId, null);
				}
			}
			return false;
		}

		public bool MoveInPreActive(int nodeId) {
			var cacheTree = GetTree(DateTime.Now, false, true);

			if(cacheTree != null) {
				var nodeItem = (cacheTree as List<Tree>).FirstOrDefault(x => x.Id == nodeId);
				if(nodeItem != null) {
					var parentId = nodeItem.ParentId;
					return _questionnaireRepository.MoveInPreActive(nodeId, parentId, QuestionnaireContext.Current.UserId, nodeItem.Comment);
				}
			}
			return false;
		}

		public bool MoveNodeUp(int nodeId) {
			var cacheTree = _cacheRepository.Fetch(Tree.CacheKey + QuestionnaireContext.Current.UserId);
			if(cacheTree != null) {
				var nodeItem = (cacheTree as List<Tree>).FirstOrDefault(x => x.Id == nodeId);
				if(nodeItem != null) {
					var parentId = nodeItem.ParentId;
					return _questionnaireRepository.MoveNodeUp(nodeId, parentId, QuestionnaireContext.Current.UserId, null);
				}
			}
			return false;
		}

		public bool RemoveTreeNode(int nodeId) {
			return _questionnaireRepository.RemoveTreeNode(nodeId, QuestionnaireContext.Current.UserId);
		}

		public bool AddNodeToFavourite(int nodeId) {
			return _questionnaireRepository.AddNodeToFavourite(nodeId, QuestionnaireContext.Current.UserId, null);
		}

		public bool RemoveNodeFromFavourite(int recordId, int nodeId) {
			return _questionnaireRepository.RemoveNodeFromFavourite(recordId, nodeId, QuestionnaireContext.Current.UserId, null);
		}

		public IEnumerable<Tree> GetTree(DateTime date, bool onlyPublished, bool loadFromCache) {
			var languageId = QuestionnaireContext.Current.LanguageId;
			var userId = QuestionnaireContext.Current.UserId;
			IEnumerable<Tree> result = new List<Tree> { };

			var treeCacheKey = string.Format("{0}_{1}_{2}_{3}_{4}", Tree.CacheKey, onlyPublished, languageId, userId, date.ToString("dd_MM_yyyy"));
			if(loadFromCache) {
				var cachedTree = _cacheRepository.Fetch(treeCacheKey);
				if(cachedTree != null) {
					result = cachedTree as List<Tree>;
				} else {
					result = _questionnaireRepository.GetFaqTree(date, onlyPublished, languageId, userId);
					_cacheRepository.Save(treeCacheKey, result);
				}
			} else {
				result = _questionnaireRepository.GetFaqTree(date, onlyPublished, languageId, userId);
				_cacheRepository.Save(treeCacheKey, result);
			}

			return result;
		}

		public IEnumerable<Tree> GetTreeForLink(int treeId) {
			return _questionnaireRepository.GetTreeForLink(treeId, QuestionnaireContext.Current.UserId, QuestionnaireContext.Current.LanguageId);
		}

		public IEnumerable<Tree> GetTreeForChange(int nodeId) {
			return _questionnaireRepository.GetFaqTreeForChange(nodeId, QuestionnaireContext.Current.LanguageId, QuestionnaireContext.Current.UserId);
		}

		public IEnumerable<Tree> GetTreeForCopy(int nodeId) {
			return _questionnaireRepository.GetFaqTreeForCopy(nodeId, QuestionnaireContext.Current.LanguageId, QuestionnaireContext.Current.UserId);
		}

		public IEnumerable<Sms> GetTreeSms(int treeId, DateTime date) {
			return _questionnaireRepository.GetTreeSms(treeId, date, QuestionnaireContext.Current.LanguageId, QuestionnaireContext.Current.UserId);
		}

		public bool SendEmail(int nodeId, string email, string emailBody) {
			//var cacheTree = _cacheRepository.Fetch(Tree.CacheKey + QuestionnaireContext.Current.UserId);
			var cacheTree = GetTree(new DateTime(1900, 1, 1), true, true);
			var treeAnswer = GetTreeAnswer(nodeId, DateTime.Now);

			if(cacheTree != null) {
				var nodeItem = (cacheTree as List<Tree>).FirstOrDefault(x => x.Id == nodeId);
				if(nodeItem != null) {
					var EmailTitle = nodeItem.FullName;
					var fromEmail = QuestionnaireContext.Current.Settings.Questionnaire.FromEmail;

					var emailSendFilePath = "";
					var emailSendFolderPath = "";
					var errorOccured = false;

					try {

						var emailSendFolderName = Guid.NewGuid().ToString("N");
						emailSendFolderPath = UtilExtenders.MapPath("~/Uploads/EmailSendTemporaryFiles/" + emailSendFolderName);
						emailSendFilePath = Path.Combine(emailSendFolderPath, nodeId + ".htm");

						File.Copy(treeAnswer.FilePath, emailSendFilePath);
					} catch(Exception ex) {
						errorOccured = true;
					}

					if(errorOccured) {
						try { Directory.Delete(emailSendFolderPath, true); } catch(Exception ex) { }
						return false;
					}

					var attachementPath = emailSendFilePath;
					emailBody = "გთხოვთ იხილოთ მიმაგრებული ფაილი";
					var emailSendResponse = _mailService.Send(fromEmail, email, EmailTitle, emailBody, attachementPath);
					if(emailSendResponse) {
						SendNotificationLog(2, 0, string.Empty, email, emailBody);
					}
					try { Directory.Delete(emailSendFolderPath, true); } catch(Exception ex) { }
					return emailSendResponse;
				}
			}
			return false;
		}

		public bool SendSms(int treeId, string number, List<string> notifications) {
			var url = "http://smsend.intra:7777/pls/sms/phttp2sms.Process?src=30530&dst=995{0}&txt={1}";

			try {
				foreach(var notification in notifications) {
					var _url = string.Format(url, number, notification);
					var request = (HttpWebRequest)WebRequest.Create(_url);
					var response = request.GetResponse();
					if(response != null) {
						var responseStream = response.GetResponseStream();
						var responseMessage = new StreamReader(responseStream).ReadToEnd();
						if(responseMessage == "Y") {
							//This sends log
							SendNotificationLog(1, treeId, number, string.Empty, notification);
							return true;
						}
					}
				}
				return false;
			} catch(Exception ex) {
				return false;
			}
		}

		public bool AddEmailAnswer(int nodeId, int answerId, DateTime fromDate, DateTime toDate, string answer, string pureAnswer, string comment, bool saveAsTemplate, string templateName) {
			var searchTags = new List<string> { };
			if(!string.IsNullOrEmpty(pureAnswer)) {
				var splitedTags = pureAnswer.Split(' ');
				var tag = "";

				foreach(var splitedTag in splitedTags) {
					if(tag.Length + splitedTag.Length <= QuestionnaireContext.Current.Settings.Questionnaire.SearchTagsLength) {
						tag += (splitedTag + ";");
					} else {
						searchTags.Add(tag); tag = string.Empty;
					}
				}
				searchTags.Add(tag);
			}

			var emailAnswerHtmlName = Guid.NewGuid().ToString("N");
			var emailAnswerPack = UtilExtenders.MapPath("~/Uploads/FaqTree/EmailAnswers");
			var emailAnswerPath = Path.Combine(emailAnswerPack, emailAnswerHtmlName + ".htm");

			using(FileStream fs = new FileStream(emailAnswerPath, FileMode.Create)) {
				using(StreamWriter w = new StreamWriter(fs, Encoding.UTF8)) {
					w.WriteLine(answer);
				}
			}

			return _questionnaireRepository.AddEmailAnswer(nodeId, answerId, fromDate, toDate, answer, emailAnswerHtmlName, searchTags, comment, saveAsTemplate, templateName, QuestionnaireContext.Current.UserId, QuestionnaireContext.Current.LanguageId);
		}

		public IEnumerable<Tree> SearchInTree(int searchType, string searchTerm, string searchComment, bool onlyPublished, DateTime date, DateTime periodStart, DateTime periodEnd, int statusId, int ownerId) {
			return _questionnaireRepository.SearchInTree(searchType, searchTerm, searchComment, onlyPublished, date, periodStart, periodEnd, statusId, ownerId, QuestionnaireContext.Current.UserId, QuestionnaireContext.Current.LanguageId);
		}

		public IEnumerable<AnswerTemplate> GetTreeEmailTemlpates(int treeId, bool loadTemplates = false) {
			var templates = _questionnaireRepository.GetTreeEmailTemlpates(treeId);
			var results = new List<AnswerTemplate> { };
			if(loadTemplates) {
				foreach(var template in templates) {
					var emailAnswerPack = UtilExtenders.MapPath("~/Uploads/FaqTree/EmailAnswers");
					var emailAnswerPath = Path.Combine(emailAnswerPack, template.FileName + ".htm");
					if(File.Exists(emailAnswerPath)) {
						var emailBody = System.IO.File.ReadAllText(emailAnswerPath);
						results.Add(new AnswerTemplate {
							Id = template.Id,
							Name = template.Name,
							FileName = template.FileName,
							Body = emailBody
						});
					} else {
						results.Add(new AnswerTemplate {
							Id = template.Id,
							Name = template.Name,
							FileName = template.FileName
						});
					}
				}
			} else {
				results.AddRange(templates);
			}
			return results;
		}

		public IEnumerable<Owner> GetOwners() {
			return _questionnaireRepository.GetOwners(QuestionnaireContext.Current.UserId, QuestionnaireContext.Current.LanguageId);
		}

		public IEnumerable<AnswerVersion> GetAnswerVersions(int nodeId) {
			return _questionnaireRepository.GetAnswerVersions(nodeId, QuestionnaireContext.Current.UserId, QuestionnaireContext.Current.LanguageId);
		}

		public IEnumerable<AnswerTemplate> GetTreeEmailTemplatesByNames(int treeId, List<string> names) {
			var results = new List<AnswerTemplate> { };
			foreach(var name in names) {
				var emailAnswerPack = UtilExtenders.MapPath("~/Uploads/FaqTree/EmailAnswers");
				var emailAnswerPath = Path.Combine(emailAnswerPack, name + ".htm");
				if(File.Exists(emailAnswerPath)) {
					var emailBody = System.IO.File.ReadAllText(emailAnswerPath);
					results.Add(new AnswerTemplate {
						Body = emailBody
					});
				}
			}
			return results;
		}

		public IEnumerable<DeletedTree> GetTreeRemovedRecords() {
			return _questionnaireRepository.GetFaqTreeDeleted(QuestionnaireContext.Current.UserId, QuestionnaireContext.Current.LanguageId);
		}

		public bool RecoverTreeFromDeletedRecords(int nodeId, string comment) {
			return _questionnaireRepository.RecoverTreeFromDeletedRecords(nodeId, comment, QuestionnaireContext.Current.UserId, QuestionnaireContext.Current.LanguageId);
		}

		public IEnumerable<Reminder> GetReminderItems() {
			return _questionnaireRepository.GetReminderItems(QuestionnaireContext.Current.UserId, QuestionnaireContext.Current.LanguageId);
		}

		public IEnumerable<string> GetEmailGroups(string number) {
			return _questionnaireRepository.GetEmailGroups(number, QuestionnaireContext.Current.UserId, QuestionnaireContext.Current.LanguageId);
		}

		public bool RegisterMsisdn(string number, int treeId) {
			return _questionnaireRepository.RegisterMsisdn(-1, treeId, number, string.Empty, QuestionnaireContext.Current.UserId, QuestionnaireContext.Current.LanguageId);
		}

		public IEnumerable<EmailComparisonFile> GetEmailComparisonFiles(int treeId, int answerId, DateTime date) {
			return _questionnaireRepository.GetEmailComparisonFiles(treeId, answerId, date, QuestionnaireContext.Current.UserId, QuestionnaireContext.Current.LanguageId);
		}

		public void SaveTreeFile(int id, DateTime fromDate, DateTime toDate, int treeId, int answerId, string fileName, string comment) {
			_questionnaireRepository.SaveTreeFile(id, fromDate, toDate, treeId, answerId, fileName, comment, QuestionnaireContext.Current.UserId, QuestionnaireContext.Current.LanguageId);
		}

		public void SaveTreeModalFile(int id, DateTime fromDate, DateTime toDate, int treeId, int answerId, string fileName, string comment) {
			_questionnaireRepository.SaveTreeModalFile(id, fromDate, toDate, treeId, answerId, fileName, comment, QuestionnaireContext.Current.UserId, QuestionnaireContext.Current.LanguageId);
		}

		public void RemoveTreeFile(int id, DateTime fromDate, string fileName) {
			_questionnaireRepository.DeleteTreeFile(id, fromDate, fileName, QuestionnaireContext.Current.UserId, QuestionnaireContext.Current.LanguageId);
		}

		public IEnumerable<GroupEmail> GetGroupEmails() {
			return _questionnaireRepository.GetGroupEmails(QuestionnaireContext.Current.UserId, QuestionnaireContext.Current.LanguageId);
		}

		public void SendGroupEmail(int treeId, string text, int sendHour, List<string> emails) {
			foreach(var email in emails) {
				_questionnaireRepository.SendGroupEmail(treeId, text, sendHour, email, QuestionnaireContext.Current.UserId, QuestionnaireContext.Current.LanguageId);
			}
		}

		public string GetCurrentCallMobileNumber() {
			var cacheMobileCallsKey = "cacheMobileCallKey";
			var cachedMobileCalls = _cacheRepository.Fetch(cacheMobileCallsKey);
			var userId = QuestionnaireContext.Current.UserId;

			if(cachedMobileCalls != null && cachedMobileCalls is Dictionary<decimal, string>) {
				var userCacheCalls = cachedMobileCalls as Dictionary<decimal, string>;
				if(userCacheCalls.ContainsKey(userId)) {
					var mobile = userCacheCalls[userId];
					return mobile;
				}
			}

			return string.Empty;
		}

		public IEnumerable<TreeStatus> GetStatuses() {
			return _questionnaireRepository.GetStatuses();
		}

		public void SendNotificationLog(int type, int treeId, string msisdn, string email, string sms) {
			_questionnaireRepository.SendNotificationLog(type, treeId, msisdn, email, sms, QuestionnaireContext.Current.UserId, QuestionnaireContext.Current.LanguageId);
		}

		public void SaveTreeNodeFile() {
			throw new NotImplementedException();
		}

		public void RemoveTreeNodeFile() {
			throw new NotImplementedException();
		}
	}
}