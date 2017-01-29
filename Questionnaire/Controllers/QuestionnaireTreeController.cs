using Newtonsoft.Json;
using Questionnaire.Domain.Interfaces.Services;
using Questionnaire.Infrastructure.Attributes;
using Questionnaire.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Questionnaire.Infrastructure.Utils;
using System.IO;
using System.Text;
using Questionnaire.Domain;
using Questionnaire.Domain.Utils;
using Questionnaire.Domain.Entities;
using System.Threading.Tasks;

namespace Questionnaire.Controllers {
	[RoutePrefix("QuestionnaireTree")]
	public class QuestionnaireTreeController : BaseAuthorizedController {
		private IQuestionnaireService _questionnaireService;
		private IDateService _dateService;

		public QuestionnaireTreeController(IDateService dateService, IQuestionnaireService questionnaireService) {
			_questionnaireService = questionnaireService;
			_dateService = dateService;
		}

		[Route("Index"), HttpGet]
		public ActionResult Index() {
			return View();
		}

		[Route("Get/{onlyPublished}/{epoch}/{loadFromCache}"), HttpGet]
		public ActionResult Get(bool onlyPublished, long epoch, bool loadFromCache) {
			var _date = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(epoch);
			var _treeItems = _questionnaireService.GetTree(_date, onlyPublished, loadFromCache).ToList();

			var _treeItemsModel = QuestionnaireTreeViewModel.ParseFrom(_treeItems).MakeParentsBold();
			var _expanded = QuesionnaireExpandedViewModel.ExpandTree(_treeItemsModel);

			var m = new QuestionnaireViewModel {
				TreeItems = _treeItemsModel,
				ExpandedItems = _expanded
			};

			QuestionnaireContext.Current.PreActiveIsChecked = onlyPublished;

			var model = JsonConvert.SerializeObject(m);
			return Json(new { data = model }, JsonRequestBehavior.AllowGet);
		}

		[Route("Controls"), HttpGet]
		public ActionResult Controls() {
			return View();
		}

		[Route("ContextMenu"), HttpGet]
		public ActionResult ContextMenu() {
			return View();
		}

		[Route("MaintainTree/{treeid}"), HttpGet]
		public ActionResult MaintainTree(int treeId) {
			return null;
		}

		#region Tree Control Actions

		[Route("AddTreeNode/{nodeId}"), HttpGet]
		public ActionResult AddTreeNode(int nodeId) {
			//Fill files ... 
			var files = new List<TreeFileViewModel> { };
			//files.AddRange(GetTreeFiles(nodeId));
			var _statuses = _questionnaireService.GetStatuses();

			var model = new MaintainTreeItemViewMode {
				//TreeModel = GetQuestionnaireViewModelForEdit(nodeId),
				TreeModel = GetQuestionnaireViewModelForAdd(),
				TreeId = nodeId,
				SelectedNodeId = nodeId,
				LockTree = false,
				//FromDateEpoch = DateTime.Now.ToEpoch(), //TODO: This needs to be configurable
				//ToDateEpoch = DateTime.Now.AddYears(1).ToEpoch(), //TODO: This needs to be configurable

				FromDateEpoch = new DateTime(2030, 1, 1).ToEpoch(),
				ToDateEpoch = new DateTime(2050, 1, 1).ToEpoch(),
				CreateDateEpoch = null,

				NewsDays = QuestionnaireContext.Current.Settings.Questionnaire.NewsDays,
				ModifyStatus = ModifyStatus.AddTreeNode,
				//Status = 1,
				//Statuses = _statuses.Select(x => (DropDownViewModel)x).ToList(),
				Files = files
			};
			return View("MaintainTree", model);
		}

		[Route("ChangeTreeNode/{nodeId}"), HttpGet]
		public ActionResult ChangeTreeNode(int nodeId) {
			//Fill files ... 
			var files = new List<TreeFileViewModel> { };
			files.AddRange(GetTreeFiles(nodeId));

			var _statuses = _questionnaireService.GetStatuses();

			var nodeList = _questionnaireService.GetTreeForChange(nodeId);
			if(nodeList != null) {
				var node = nodeList.FirstOrDefault(x => x.Id == nodeId);
				//TODO: needs to be implemented
				var model = new MaintainTreeItemViewMode {
					TreeModel = GetQuestionnaireViewModelForEdit(nodeId),
					TreeId = nodeId,
					SelectedNodeId = node.ParentId,
					PreviousChangeMadeEditor = node.UserName,
					LockTree = false,
					ModifyStatus = ModifyStatus.ChangeTreeNode,
					FromDateEpoch = node.FromDate.ToEpoch(),
					ToDateEpoch = node.ToDate.ToEpoch(),
					CreateDateEpoch = node.CreateDate.HasValue ? node.CreateDate.Value.ToEpoch() : null as double?,
					Text = node.Text,
					Comment = node.Comment,
					NewsDays = node.NewsDays,
					//Status = _statuses.FirstOrDefault(x => x.Name == node.Status).Id,
					//Statuses = _statuses.Select(x => (DropDownViewModel)x).ToList(),
					Files = files
				};
				return View("MaintainTree", model);
			}
			return null;
		}

		[Route("MoveNodeUp/{nodeId}"), HttpGet]
		public ActionResult MoveNodeUp(int nodeId) {
			return JsonResult(_questionnaireService.MoveNodeUp(nodeId));
		}

		[Route("MoveNodeDown/{nodeId}"), HttpGet]
		public ActionResult MoveNodeDown(int nodeId) {
			return JsonResult(_questionnaireService.MoveNodeDown(nodeId));
		}

		[Route("MoveInPreActive/{nodeId}"), HttpGet]
		public ActionResult MoveInPreActive(int nodeId) {
			return JsonResult(_questionnaireService.MoveInPreActive(nodeId));
		}

		[Route("CopyTreeNode/{nodeId}"), HttpGet]
		public ActionResult CopyTreeNode(int nodeId) {
			//Fill files ... 
			var files = new List<TreeFileViewModel> { };
			files.AddRange(GetTreeFiles(nodeId));

			var _statuses = _questionnaireService.GetStatuses();

			var nodeList = _questionnaireService.GetTreeForChange(nodeId);
			if(nodeList != null) {
				var node = nodeList.FirstOrDefault(x => x.Id == nodeId);
				//TODO: needs to be implemented
				var model = new MaintainTreeItemViewMode {
					TreeModel = GetQuestionnaireViewModelForEdit(nodeId),
					TreeId = nodeId,
					SelectedNodeId = node.ParentId,
					LockTree = false,
					AllowCopyChildrens = true,
					ModifyStatus = ModifyStatus.CopyTreeNode,
					PreviousChangeMadeEditor = node.UserName,
					FromDateEpoch = node.FromDate.ToEpoch(),
					ToDateEpoch = node.ToDate.ToEpoch(),
					CreateDateEpoch = node.CreateDate.HasValue ? node.CreateDate.Value.ToEpoch() : null as double?,
					Text = node.Text,
					Comment = node.Comment,
					NewsDays = node.NewsDays,
					//Status = _statuses.FirstOrDefault(x => x.Name == node.Status).Id,
					//Statuses = _statuses.Select(x => (DropDownViewModel)x).ToList(),
					Files = files
				};
				return View("MaintainTree", model);
			}
			return null;
		}

		[Route("MoveTreeNode/{nodeId}"), HttpGet]
		public ActionResult MoveTreeNode(int nodeId) {
			//Fill files ... 
			var files = new List<TreeFileViewModel> { };
			files.AddRange(GetTreeFiles(nodeId));

			var _statuses = _questionnaireService.GetStatuses();

			var nodeList = _questionnaireService.GetTreeForChange(nodeId);
			if(nodeList != null) {
				var node = nodeList.FirstOrDefault(x => x.Id == nodeId);
				//TODO: needs to be implemented
				var model = new MaintainTreeItemViewMode {
					TreeModel = GetQuestionnaireViewModelForEdit(nodeId),
					SelectedNodeId = node.ParentId,
					TreeId = nodeId,
					LockTree = false,
					PreviousChangeMadeEditor = node.UserName,
					ModifyStatus = ModifyStatus.MoveTreeNode,
					FromDateEpoch = node.FromDate.ToEpoch(),
					ToDateEpoch = node.ToDate.ToEpoch(),
					CreateDateEpoch = node.CreateDate.HasValue ? node.CreateDate.Value.ToEpoch() : null as double?,
					Text = node.Text,
					Comment = node.Comment,
					NewsDays = node.NewsDays,
					//Status = _statuses.FirstOrDefault(x => x.Name == node.Status).Id,
					//Statuses = _statuses.Select(x => (DropDownViewModel)x).ToList(),
					Files = files
				};
				return View("MaintainTree", model);
			}
			return null;
		}

		[Route("RemoveTreeNode/{nodeId}"), HttpGet]
		public ActionResult RemoveTreeNode(int nodeId) {
			return JsonResult(_questionnaireService.RemoveTreeNode(nodeId));
		}

		[Route("AddNodeToFavourite/{nodeId}"), HttpGet]
		public ActionResult AddNodeToFavourite(int nodeId) {
			return JsonResult(_questionnaireService.AddNodeToFavourite(nodeId));
		}

		[Route("RemoveFromFavourite/{recordId}/{nodeId}"), HttpGet]
		public ActionResult RemoveFromFavourite(int recordId, int nodeId) {
			return JsonResult(_questionnaireService.RemoveNodeFromFavourite(recordId, nodeId));
		}

		private QuestionnaireViewModel GetQuestionnaireViewModelForEdit(int nodeId) {
			//var _date = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(-2208988800);
			//var _treeItems = _questionnaireService.GetTree(_date, true, true).ToList();

			var _treeItems = _questionnaireService.GetTreeForCopy(nodeId).ToList();

			var _treeItemsModel = QuestionnaireTreeViewModel.ParseFrom(_treeItems);
			var _expanded = QuesionnaireExpandedViewModel.ExpandTree(_treeItemsModel);

			return new QuestionnaireViewModel {
				TreeItems = _treeItemsModel,
				ExpandedItems = _expanded
			};
		}

		private QuestionnaireViewModel GetQuestionnaireViewModelForAdd() {
			var _date = new DateTime(1900, 1, 1, 0, 0, 0, 0);
			var _treeItems = _questionnaireService.GetTree(_date, true, true).ToList();

			var _treeItemsModel = QuestionnaireTreeViewModel.ParseFrom(_treeItems);
			var _expanded = QuesionnaireExpandedViewModel.ExpandTree(_treeItemsModel);

			return new QuestionnaireViewModel {
				TreeItems = _treeItemsModel,
				ExpandedItems = _expanded
			};
		}

		#endregion

		[Route("SaveTree/{nodeId}"), HttpPost]
		public ActionResult SaveTree(int nodeId, MaintainTreeItemViewMode model) {
			var comment = model.Comment;
			var fromDate = model.FromDateEpoch.Value.FromEpoch();
			var toDate = model.ToDateEpoch.Value.FromEpoch();
			var text = model.Text;
			var treeId = nodeId;
			var newsDays = model.NewsDays;
			var treeToId = model.TreeToId;
			//var statusId = (int)model.Status.Value;
			var statusId = 1;
			var copyChildrens = model.CopyChildrens;

			var result = true;

			switch(model.ModifyStatus) {
				case ModifyStatus.AddTreeNode:
					//QuestionnaireContext.Current.SelectedNodeId = 1;
					var addedTreeId = _questionnaireService.AddTreeNode(treeId, text, statusId, fromDate, toDate, newsDays.Value, comment);
					result = addedTreeId != 0;
					treeId = Convert.ToInt32(addedTreeId);
					nodeId = treeId;
					break;
				case ModifyStatus.ChangeTreeNode:
					QuestionnaireContext.Current.SelectedNodeId = treeId;
					result = _questionnaireService.ChangeTreeNode(treeId, text, statusId, fromDate, toDate, newsDays.Value, comment);
					break;
				case ModifyStatus.CopyTreeNode:
					QuestionnaireContext.Current.SelectedNodeId = treeId;
					result = _questionnaireService.CopyTreeNode(treeId, treeToId.Value, copyChildrens, text, statusId, fromDate, toDate, newsDays.Value, comment);
					break;
				case ModifyStatus.MoveTreeNode:
					QuestionnaireContext.Current.SelectedNodeId = treeId;
					result = _questionnaireService.MoveTreeNode(treeId, treeToId.Value, text, statusId, fromDate, toDate, newsDays.Value, comment);
					break;
				default:
					break;
			}

			if(model.Files != null && model.Files.Count > 0 && result) {
				//Save files ...
				var filesToStore = model.Files.Where(x => x.IsTemporary);
				foreach(var file in filesToStore) {
					var folderPath = Path.Combine("~/Uploads/TemporaryUploads", file.FolderName);
					var filePath = Path.Combine(folderPath, file.FileName);

					var folderPhisicalPath = UtilExtenders.MapPath(folderPath);
					var filePhisicalPath = Path.Combine(folderPhisicalPath, file.FileName);
					var folderDestinationPath = UtilExtenders.MapPath("~/Uploads/TreeFiles/" + treeId + "/" + file.FolderName);
					var fileDestinationPath = Path.Combine(folderDestinationPath, file.FileName);

					_questionnaireService.SaveTreeModalFile(-1, fromDate, toDate, nodeId, 0, file.FolderName, file.Comment); // Save file to db

					if(System.IO.File.Exists(filePhisicalPath)) {
						System.IO.File.Copy(filePhisicalPath, fileDestinationPath);
						System.IO.Directory.Delete(folderPhisicalPath, true);
					}
				}
			}

			return JsonResult(new { treeId = treeId }, result);
		}

		[Route("EmailAnswerControl"), HttpGet]
		public ActionResult EmailAnswerControl() {
			var model = new EmailAnswerControlViewModel { Emails = new List<string> { } };

			//model.Emails.Add("test@mail.com");

			return View(model);
		}

		[Route("SendEmail/{nodeId}/{email}"), HttpGet]
		public ActionResult SendEmail(int nodeId, string email) {
			//_questionnaireService.SendEmail(nodeId, email); 
			return View();
		}

		[Route("GetEmailNotification/{nodeId}/{isHistoryOn}/{historyDateEpoch}"), HttpGet]
		public ActionResult GetEmailNotification(int nodeId, bool isHistoryOn, double historyDateEpoch) {
			var historyDate = DateTime.Now;
			if(isHistoryOn) {
				if(historyDateEpoch > 0) {
					historyDate = historyDateEpoch.FromEpoch();
				}
			}

			var answer = (EmailNotificationViewModel)_questionnaireService.GetTreeAnswer(nodeId, historyDate);
			return View(answer);
		}

		[Route("ViewEmailNotification/{nodeId}"), HttpGet]
		public ActionResult ViewEmailNotification(int nodeId) {
			var answer = (EmailNotificationViewModel)_questionnaireService.GetTreeAnswer(nodeId, DateTime.Now);
			return View(answer);
		}

		[Route("SendEmailNotification/{nodeId}"), HttpPost, ValidateInput(false)]
		public ActionResult SendEmailNotification(int nodeId, EmailAnswerControlViewModel model) {
			return JsonResult(_questionnaireService.SendEmail(nodeId, model.Email, model.EmailBody));
		}

		[Route("AddEmailAnswer/{nodeId}"), HttpGet]
		public ActionResult AddEmailAnswer(int nodeId) {
			var answer = _questionnaireService.GetTreeAnswer(nodeId, DateTime.Now);
			var templates = _questionnaireService.GetTreeEmailTemlpates(nodeId)
																					 .Select(x => (EmailTemplate)x).ToList();

			var files = new List<EmailAnswerFile> { };
			var dbFiles = _questionnaireService.GetEmailComparisonFiles(nodeId, answer.AnswerId, DateTime.Now);
			if(dbFiles.Count() > 0) {
				foreach(var file in dbFiles) {
					var fileName = GetEmailAnswerFileName(file.TreeId, file.FolderName);
					if(!string.IsNullOrEmpty(fileName)) {
						files.Add(new EmailAnswerFile {
							Id = file.Id,
							AnswerId = answer.AnswerId,
							FolderName = file.FolderName,
							Comment = file.Comment,
							Name = fileName,
							IsTemporary = false
						});
					}
				}
			}

			var hours = new List<DropDownViewModel> { };
			for(int i = 0; i < 24; i++) { hours.Add(new DropDownViewModel { Id = i, Name = string.Format("{0}:00", i) }); }

			var emailList = _questionnaireService.GetGroupEmails().Select(x => (DropDownCheckboxViewModel)x).ToList();

			var model = new AddEmailAnswerModal {
				NodeId = nodeId,
				AnswerId = answer.AnswerId,
				Answer = answer.Body,
				FromDateEpoch = (answer.FromDate == default(DateTime) ? DateTime.Now : answer.FromDate).ToEpoch(),
				ToDateEpoch = (answer.ToDate == default(DateTime) ? new DateTime(2100, 1, 1) : answer.ToDate).ToEpoch(),
				Comment = answer.Comment,
				Templates = templates,
				Files = files,
				HourId = 17,
				Hours = hours,
				EmailList = emailList
			};
			return View(model);
		}

		[Route("AddEmailAnswer/{nodeId}"), HttpPost, ValidateInput(false)]
		public ActionResult AddEmailAnswer(int nodeId, AddEmailAnswerModal model) {
			var fromDate = model.FromDateEpoch.Value.FromEpoch();
			var toDate = model.ToDateEpoch.Value.FromEpoch();
			var answer = (model.Answer ?? "").Replace("<table", "<table border='1'");
			var answerId = model.AnswerId ?? -1;
			var pureAnswer = model.PureAnswer;
			var comment = model.Comment;
			var saveAsTemplate = model.SaveAsTemplate;
			var templateName = model.TemplateName;

			var result = _questionnaireService.AddEmailAnswer(nodeId, answerId, fromDate, toDate, answer, pureAnswer, comment, saveAsTemplate, templateName);

			Answer _answerObject = new Answer { };
			if(answerId == -1) {
				_answerObject = _questionnaireService.GetTreeAnswer(nodeId, DateTime.Now);
			}

			if(model.Files != null && model.Files.Count > 0 && result) {
				//Save files ...
				var filesToStore = model.Files.Where(x => x.IsTemporary);
				foreach(var file in filesToStore) {
					var folderPath = Path.Combine("~/Uploads/TemporaryUploads", file.FolderName);
					var filePath = Path.Combine(folderPath, file.Name);

					var folderPhisicalPath = UtilExtenders.MapPath(folderPath);
					var filePhisicalPath = Path.Combine(folderPhisicalPath, file.Name);
					var folderDestinationPath = UtilExtenders.MapPath("~/Uploads/EmailAnswerFiles/" + model.NodeId + "/" + file.FolderName);
					var fileDestinationPath = Path.Combine(folderDestinationPath, file.Name);

					_questionnaireService.SaveTreeFile(-1, fromDate, toDate, nodeId, answerId == -1 ? _answerObject.AnswerId : answerId, file.FolderName, file.Comment); // Save file to db																																																																														

					if(System.IO.File.Exists(filePhisicalPath)) {
						System.IO.File.Copy(filePhisicalPath, fileDestinationPath);
						System.IO.Directory.Delete(folderPhisicalPath, true);
					}
				}
			}

			var emails = model.EmailList.Where(x => x.IsChecked).ToList();
			if(emails.Count > 0) {
				var emailList = emails.Select(x => x.Name).ToList();
				var emailText = "/QuestionnaireTree/ViewEmailNotification/" + model.NodeId;
				_questionnaireService.SendGroupEmail(model.NodeId.Value, emailText, model.HourId, emailList);
			}

			return JsonResult(result);
		}

		[Route("GetEmailTemplate/{nodeId}/{templateFileName}"), HttpGet]
		public ActionResult GetEmailTemplate(int nodeId, string templateFileName) {
			var templates = _questionnaireService.GetTreeEmailTemlpates(nodeId, true);
			var template = templates.FirstOrDefault(x => x.FileName == templateFileName);
			if(template != null) {
				return JsonResult(new { templateBody = template.Body });
			}
			return JsonResult(false);
		}

		[Route("GetAnswerTagsModal/{nodeId}"), HttpGet]
		public ActionResult GetAnswerTagsModal(int nodeId) {
			var model = new AnswerTagModel { NodeId = nodeId, TreeNodes = new List<AnswerTagTreeModel> { } };
			var treeItems = _questionnaireService.GetTreeForLink(nodeId);

			foreach(var treeItem in treeItems) {
				model.TreeNodes.Add(new AnswerTagTreeModel {
					Id = treeItem.Id,
					Text = treeItem.Text
				});
			}

			return View(model);
		}

		[Route("SearchInTree/{searchType}/{searchText}"), HttpGet]
		public ActionResult SearchInTree(int searchType, string searchText) {
			var _treeItems = _questionnaireService.SearchInTree(searchType, searchText, /*Fake values-->>*/string.Empty, false, DateTime.Now, DateTime.Now, DateTime.Now, 1, 1/*<<--Fake values*/).ToList();

			var _treeItemsModel = QuestionnaireTreeViewModel.ParseFrom(_treeItems);
			var _expanded = QuesionnaireExpandedViewModel.ExpandTree(_treeItemsModel);

			var m = new QuestionnaireViewModel {
				TreeItems = _treeItemsModel,
				ExpandedItems = _expanded
			};

			var model = JsonConvert.SerializeObject(m);

			return Json(new { data = model }, JsonRequestBehavior.AllowGet);
		}

		[Route("ExtendedSearch"), HttpGet]
		public ActionResult ExtendedSearch() {
			var _statuses = _questionnaireService.GetStatuses();
			var _owners = _questionnaireService.GetOwners().ToList();
			var model = new ExtendedSearchModel {
				Owners = DropDownViewModel.GetList(_owners),
				OnlyPublishedList = DropDownViewModel.GetList<YesOrNoStatusEnum>(),
				Statuses = _statuses.Select(x => (DropDownViewModel)x).ToList(),
				PeriodStartEpoch = DateTime.Now.AddMonths(-1).ToEpoch(),
				PeriodEndEpoch = DateTime.Now.AddMonths(1).ToEpoch()
			};
			return View(model);
		}

		[Route("ExtendedSearch"), HttpPost]
		public ActionResult ExtendedSearch(ExtendedSearchModel model) {
			var searchType = model.SearchType;
			var searchText = model.SearchTerm;
			var searchComment = model.SearchComment;
			var onlyPublished = model.OnlyPublished == 1;

			var historyDate = DateTime.Now;
			if(model.IsHistoryOn) {
				if(model.HistoryDate > 0) {
					historyDate = model.HistoryDate.FromEpoch();
				}
			}

			var periodStart = model.FromToIsActive ? model.PeriodStartEpoch.Value.FromEpoch() : new DateTime(2001, 1, 1);
			var periodEnd = model.FromToIsActive ? model.PeriodEndEpoch.Value.FromEpoch() : new DateTime(2001, 1, 1);
			var statusId = model.StatusIsActive ? model.StatusId : 0;
			var ownerId = model.OwnerIsActive ? model.OwnerId : 0;

			var _treeItems = _questionnaireService.SearchInTree(searchType, searchText, searchComment, onlyPublished, historyDate, periodStart, periodEnd, statusId, ownerId).ToList();

			var _treeItemsModel = QuestionnaireTreeViewModel.ParseFrom(_treeItems);
			var _expanded = QuesionnaireExpandedViewModel.ExpandTree(_treeItemsModel);

			var m = new QuestionnaireViewModel {
				TreeItems = _treeItemsModel,
				ExpandedItems = _expanded
			};

			var response = JsonConvert.SerializeObject(m);

			return JsonResult(response, true);
		}

		[Route("GetEmailAnswersList/{nodeId}"), HttpGet]
		public ActionResult GetEmailAnswersList(int nodeId) {
			var answerVersions = _questionnaireService.GetAnswerVersions(nodeId);

			answerVersions = answerVersions.OrderBy(x => x.FromDate);
			var model = new GetEmailAnswerListView {
				TreeId = nodeId,
				Answers = answerVersions.Select(x => (EmailAnswerComparisonItem)x).ToList()
			};

			return View(model);
		}

		[Route("CompareEmailAnswers/{nodeId}"), HttpPost]
		public ActionResult CompareEmailAnswers(int nodeId, GetEmailAnswerListView model) {
			var selectedAnswers = model.Answers.Where(x => x.Checked);
			var answersToCompare = selectedAnswers.Select(x => x.Filename).ToList();
			var emailAnswerTemplates = _questionnaireService.GetTreeEmailTemplatesByNames(nodeId, answersToCompare).ToArray();

			if(emailAnswerTemplates.Count() == 2) {
				var answerOne = emailAnswerTemplates[0].Body.StripHTML();
				var answerTwo = emailAnswerTemplates[1].Body.StripHTML();

				var oldSelectedAnswer = selectedAnswers.ToArray()[0];
				var newSelectedAnswer = selectedAnswers.ToArray()[1];

				var oldFiles = _questionnaireService.GetEmailComparisonFiles(nodeId, oldSelectedAnswer.Id, oldSelectedAnswer.FromDateEpoch.Value.FromEpoch());
				var newFiles = _questionnaireService.GetEmailComparisonFiles(nodeId, newSelectedAnswer.Id, newSelectedAnswer.FromDateEpoch.Value.FromEpoch());
				var _oldFiles = new List<CompareEmailFileViewModel> { };
				var _newFiles = new List<CompareEmailFileViewModel> { };

				if(oldFiles != null && oldFiles.Count() > 0) {
					_oldFiles = oldFiles.Select(x => (CompareEmailFileViewModel)x).ToList();
				}

				if(newFiles != null && newFiles.Count() > 0) {
					_newFiles = newFiles.Select(x => (CompareEmailFileViewModel)x).ToList();
				}

				var response = new CompareEmailAnswersModel {
					OldTemplateCaption = oldSelectedAnswer.FromDate.ToString("dd.MM.yyyy HH:mm:ss"),
					NewTemplateCaption = newSelectedAnswer.FromDate.ToString("dd.MM.yyyy HH:mm:ss"),

					OldTemplateComment = oldSelectedAnswer.Comment,
					NewTemplateComment = newSelectedAnswer.Comment,

					OldTemplateUsername = oldSelectedAnswer.Username,
					NewTemplateUsername = newSelectedAnswer.Username,

					Difference = DifferenceFinder.GetDifferences(answerOne, answerTwo),
					TreeId = nodeId,
					OldFiles = _oldFiles,
					NewFiles = _newFiles
				};

				return View(response);
			} else {
				return View();
			}
		}

		[Route("SendDemoXos"), HttpPost]
		public ActionResult SendDemoXos(List<string> smsList) {
			return JsonResult(true);
		}

		[Route("RemoveEmailFiles/{id}/{nodeId}/{fileName}/{folderName}/{isTemporary}"), HttpGet]
		public ActionResult RemoveEmailFiles(int id, int nodeId, string fileName, string folderName, bool isTemporary) {

			var folderPath = isTemporary ?
															Path.Combine("~/Uploads/TemporaryUploads", folderName) :
															Path.Combine("~/Uploads/EmailAnswerFiles", nodeId.ToString(), folderName);

			var filePath = Path.Combine(folderPath, fileName);

			var folderPhisicalPath = UtilExtenders.MapPath(folderPath);
			var filePhisicalPath = Path.Combine(folderPhisicalPath, fileName);

			if(System.IO.Directory.Exists(folderPhisicalPath) && System.IO.File.Exists(filePhisicalPath)) {
				_questionnaireService.RemoveTreeFile(id, DateTime.Now, folderName);
				return JsonResult();
			}
			return JsonResult(false);
		}

		[Route("RemoveTreeFiles/{nodeId}/{folderId}/{fileName}/{folderName}/{isTemporary}"), HttpGet]
		public ActionResult RemoveTreeFiles(int nodeId, int folderId, string fileName, string folderName, bool isTemporary) {
			var folderPath = isTemporary ?
												Path.Combine("~/Uploads/TemporaryUploads", folderName) :
												Path.Combine("~/Uploads/TreeFiles", nodeId.ToString(), folderName);

			var filePath = Path.Combine(folderPath, fileName);

			var folderPhisicalPath = UtilExtenders.MapPath(folderPath);
			var filePhisicalPath = Path.Combine(folderPhisicalPath, fileName);

			if(System.IO.File.Exists(filePhisicalPath)) {
				_questionnaireService.RemoveTreeFile(folderId, DateTime.Now, folderName);
				return JsonResult();
			}
			return JsonResult(false);
		}

		[Route("TreeRemovedRecords"), HttpGet]
		public ActionResult TreeRemovedRecords() {
			var treeRemovedRecords = _questionnaireService.GetTreeRemovedRecords().Select(x => (DeletedTreeItemViewModel)x).ToList();
			var model = new DeletedTreeItemsViewModel {
				Records = treeRemovedRecords
			};
			return View(model);
		}

		[Route("RecoverFromDeletedItem/{nodeId}"), HttpGet]
		public ActionResult RecoverFromDeletedItem(int nodeId) {
			var result = _questionnaireService.RecoverTreeFromDeletedRecords(nodeId, string.Empty);
			return JsonResult(result);
		}

		[Route("Reminder"), HttpGet]
		public ActionResult Reminder() {
			var reminderItems = _questionnaireService.GetReminderItems().Select(x => (ReminderItemViewModel)x).ToList();
			var model = new ReminderItemsViewModel {
				Records = reminderItems
			};
			return View(model);
		}

		[Route("GetFileUploadDialog/{treeId}"), HttpGet]
		public ActionResult GetFileUploadDialog(int treeId) {
			var model = new GetFileUploadDialogModel {
				TreeId = treeId
			};
			return View(model);
		}

		[Route("GetAnswerFileUploadDialog/{treeId}/{answerId}"), HttpGet]
		public ActionResult GetAnswerFileUploadDialog(int treeId, int answerId) {
			var model = new GetTreeFileUploadDialogModel {
				TreeId = treeId
			};
			return View(model);
		}

		[Route("GroupedOperationRemove"), HttpPost]
		public ActionResult GroupedOperationRemove(List<int> treeIds) {
			treeIds = treeIds.Distinct().ToList();
			var taskExecutionStatuses = new Dictionary<int, bool>();
			//var treeRemoveTasks = new List<Task> { };

			foreach(var treeId in treeIds) {
				var removeResponse = _questionnaireService.RemoveTreeNode(treeId);
				taskExecutionStatuses.Add(treeId, removeResponse);

				//var removeTask = new Task(() => {
				//	var removeResponse = _questionnaireService.RemoveTreeNode(treeId);
				//	taskExecutionStatuses.Add(treeId, removeResponse);
				//});
				//treeRemoveTasks.Add(removeTask);
				//removeTask.Start();
			}

			//Task.WaitAll(treeRemoveTasks.ToArray());

			var notRemovedTreeIds = taskExecutionStatuses.Where(x => !x.Value).ToList();
			if(notRemovedTreeIds.Count > 0) {
				if(notRemovedTreeIds.Count == treeIds.Count) { //Not any trees were not deleted
					return JsonResult(new { notRemovedTreeIds = notRemovedTreeIds }, false, "დაფიქსირდა ტექნიკური შეცდომა, გთხოვთ სცადოთ მოგვიანებით");
				} else { //There are some trees which were not removed
					return JsonResult(new { notRemovedTreeIds = notRemovedTreeIds, warningMessage = "ზოგიერთი ხის წაშლისას დაფიქსირდა შეცდომა" }, true);
				}
			} else { //All trees are removed successfully
				return JsonResult(new { notRemovedTreeIds = new List<int> { } }, true);
			}
		}

		[Route("GroupedOperationMoveToPreactive"), HttpPost]
		public ActionResult GroupedOperationMoveToPreactive(List<int> treeIds) {
			treeIds = treeIds.Distinct().ToList();
			var taskExecutionStatuses = new Dictionary<int, bool>();
			//var treeMovePreactiveTasks = new List<Task> { };

			foreach(var treeId in treeIds) {
				var moveToPreActiveResponse = _questionnaireService.MoveInPreActive(treeId);
				taskExecutionStatuses.Add(treeId, moveToPreActiveResponse);

				//var removeTask = new Task(() => {
				//	var moveToPreActiveResponse = _questionnaireService.MoveInPreActive(treeId);
				//	taskExecutionStatuses.Add(treeId, moveToPreActiveResponse);
				//});
				//treeMovePreactiveTasks.Add(removeTask);
				//removeTask.Start();
			}

			//Task.WaitAll(treeMovePreactiveTasks.ToArray());

			var notMovedToPreActiveTreeIds = taskExecutionStatuses.Where(x => !x.Value).ToList();
			if(notMovedToPreActiveTreeIds.Count > 0) {
				if(notMovedToPreActiveTreeIds.Count == treeIds.Count) { //Not any trees were not deleted
					return JsonResult(new { notMovedToPreActiveTreeIds = notMovedToPreActiveTreeIds }, false, "დაფიქსირდა ტექნიკური შეცდომა, გთხოვთ სცადოთ მოგვიანებით");
				} else { //There are some trees which were not removed
					return JsonResult(new { notMovedToPreActiveTreeIds = notMovedToPreActiveTreeIds, warningMessage = "ზოგიერთი ხის პრე-აქთივში გადატანისას დაფიქსირდა შეცდომა" }, true);
				}
			} else { //All trees are removed successfully
				return JsonResult(new { notMovedToPreActiveTreeIds = new List<int> { } }, true);
			}
		}

		private List<TreeFileViewModel> GetTreeFiles(int nodeId) {
			var files = new List<TreeFileViewModel> { };
			var filesPath = UtilExtenders.MapPath("~/Uploads/TreeFiles/" + nodeId);
			var directories = System.IO.Directory.GetDirectories(filesPath);

			var dbFolders = _questionnaireService.GetEmailComparisonFiles(nodeId, 0, DateTime.Now);
			var treeFolderNames = dbFolders.Select(x => x.FolderName).ToList();

			foreach(var file in directories) {
				var directoryInfo = new DirectoryInfo(file);
				var dbDir = dbFolders.FirstOrDefault(x => x.FolderName == directoryInfo.Name);
				if(dbDir != null) {
					var directoryFiles = System.IO.Directory.GetFiles(file);

					if(directoryFiles.Count() > 0) {
						var fileInfo = new System.IO.FileInfo(directoryFiles.FirstOrDefault());

						files.Add(new TreeFileViewModel {
							Id = dbDir.Id,
							FolderName = directoryInfo.Name,
							IsTemporary = false,
							FileName = fileInfo.Name,
							Comment = dbDir.Comment
						});
					}
				}
			}
			return files;
		}

		private string GetEmailAnswerFileName(int nodeId, string folderName) {
			var filesPath = UtilExtenders.MapPath("~/Uploads/EmailAnswerFiles/" + nodeId + "/" + folderName);
			var files = System.IO.Directory.GetFiles(filesPath);

			if(files.Count() > 0) {
				var fileInfo = new System.IO.FileInfo(files.FirstOrDefault());
				return fileInfo.Name;
			}
			return string.Empty;
		}
	}
}