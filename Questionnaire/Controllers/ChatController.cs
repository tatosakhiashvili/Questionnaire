using Questionnaire.Domain;
using Questionnaire.Domain.Interfaces.Services;
using Questionnaire.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using Questionnaire.Infrastructure.Utils;
using System.Collections.Generic;
using Questionnaire.Domain.Utils;
using System.IO;

namespace Questionnaire.Controllers {
	[RoutePrefix("Chat")]
	public class ChatController : BaseAuthorizedController {

		private IChatService _chatService;

		public ChatController(IChatService chatService) {
			_chatService = chatService;
		}

		[Route(""), HttpGet]
		public ActionResult Index() {
			var messages = _chatService.GetInternalMessages();
			var model = new ChatItemsViewModel {
				Records = messages.Select(x => (ChatItemViewModel)x).ToList()
			};
			return View(model);
		}

		[Route("GetMessageDetails/{id}"), HttpGet]
		public ActionResult GetMessageDetails(int id) {
			var _statuses = _chatService.GetInternalMessageStatuses(id);

			var model = new ChatItemDetailsViewModel {
				MessageId = id,
				Statuses = _statuses.Select(x => (ChatItemStatusViewModel)x).ToList()
			};

			return View(model);
		}

		[Route("AddMessage"), HttpGet]
		public ActionResult AddMessage() {
			var _messagePriorities = _chatService.GetInternalMessagePriorities().Select(x => (DropDownViewModel)x).ToList();
			var _messageTypes = _chatService.GetInternalMessageTypes().Select(x => (DropDownViewModel)x).ToList();

			var model = new MaintainMessageViewModel {
				Priorities = _messagePriorities,
				Types = _messageTypes,
				Priority = 26,
				Type = 28,
				ProcessDateEpoch = DateTime.Now.AddDays(3).ToEpoch(),
				ModifyStatus = MessageModifyStatus.Add
			};

			return View("MaintainMessage", model);
		}

		[Route("ChangeMessage/{id}"), HttpGet]
		public ActionResult ChangeMessage(int id) {
			var _messagePriorities = _chatService.GetInternalMessagePriorities().Select(x => (DropDownViewModel)x).ToList();
			var _messageTypes = _chatService.GetInternalMessageTypes().Select(x => (DropDownViewModel)x).ToList();
			var message = _chatService.GetInternalMessage(id);

			var model = new MaintainMessageViewModel {
				Id = id,
				Text = message.Text,
				ModifyStatus = MessageModifyStatus.Change,
				Priorities = _messagePriorities,
				Types = _messageTypes,
				Priority = message.PriorityId,
				Type = message.TypeId,
				ProcessDateEpoch = message.ProcessDate.ToEpoch(),
				Comment = message.Comment
			};
			return View("MaintainMessage", model);
		}

		[Route("ExecuteMessage/{id}"), HttpGet]
		public ActionResult ExecuteMessage(int id) {
			var model = new MaintainMessageViewModel {
				Id = id,
				ModifyStatus = MessageModifyStatus.Execute,
			};
			return View("MaintainMessage", model);
		}

		[Route("DeleteMessage/{id}"), HttpGet]
		public ActionResult DeleteMessage(int id) {
			var model = new MaintainMessageViewModel {
				Id = id,
				ModifyStatus = MessageModifyStatus.Delete,
			};
			return View("MaintainMessage", model);
		}

		[Route("MaintainMessage"), HttpPost]
		public ActionResult MaintainMessage(MaintainMessageViewModel model) {
			var maintainMessageResult = false;
			var chatId = model.Id;

			switch(model.ModifyStatus) {
				case MessageModifyStatus.Add:
					var addMessageResult = _chatService.AddInternalMessage(model.Text, model.Type, model.Priority, model.Comment, DateTime.Now, DateTime.Now, model.ProcessDateEpoch.Value.FromEpoch());
					chatId = addMessageResult;
					maintainMessageResult = addMessageResult > 0;
					break;
				case MessageModifyStatus.Change:
					maintainMessageResult = _chatService.ChangeInternalMessage(model.Id, model.Text, model.Type, model.Priority, model.Comment, DateTime.Now, DateTime.Now, model.ProcessDateEpoch.Value.FromEpoch());
					break;
				case MessageModifyStatus.Execute:
					maintainMessageResult = _chatService.ExecuteInternalMessage(model.Id, model.Comment);
					break;
				case MessageModifyStatus.Delete:
					maintainMessageResult = _chatService.DeleteInternalMessage(model.Id, model.Comment);
					break;
				default:
					break;
			}

			if(maintainMessageResult && (model.ModifyStatus == MessageModifyStatus.Add || model.ModifyStatus == MessageModifyStatus.Change) && model.Files != null) {
				var filesToStore = model.Files.Where(x => x.IsTemporary);

				foreach(var file in filesToStore) {
					var folderPath = Path.Combine("~/Uploads/TemporaryUploads", file.FolderName);
					var filePath = Path.Combine(folderPath, file.FileName);

					var folderPhisicalPath = UtilExtenders.MapPath(folderPath);
					var filePhisicalPath = Path.Combine(folderPhisicalPath, file.FileName);
					var folderDestinationPath = UtilExtenders.MapPath("~/Uploads/ChatFiles/" + chatId + "/" + file.FolderName);
					var fileDestinationPath = Path.Combine(folderDestinationPath, file.FileName);

					_chatService.SaveFile(chatId, file.FolderName);

					if(System.IO.File.Exists(filePhisicalPath)) {
						System.IO.File.Copy(filePhisicalPath, fileDestinationPath);
						System.IO.Directory.Delete(folderPhisicalPath, true);
					}
				}
			}

			return JsonResult(maintainMessageResult);
		}

		[Route("MessageFiles/{messageId}/{showControls}/{showSearchBox}"), HttpGet]
		public ActionResult MessageFiles(int messageId, bool showControls, bool showSearchBox) {
			var _files = _chatService.GetInternalMessageFiles(messageId).Select(x => (ChatItemFileViewModel)x).ToList();

			var model = new MessageFileViewModel {
				MessageId = messageId,
				ShowControls = showControls,
				ShowSearchBox = showSearchBox,
				Files = _files
			};
			return View(model);
		}

		[Route("UploadFile"), HttpPost]
		public ActionResult UploadFile() {
			if(Request.Files.Count > 0) {
				var file = Request.Files[0];
				var fileName = file.FileName;
				var folderName = Guid.NewGuid().ToString("N");

				var uploadPath = UtilExtenders.MapPath("~/Uploads/TemporaryUploads/" + folderName);
				var filePath = System.IO.Path.Combine(uploadPath, fileName);

				var fileData = GetFileData(file);

				System.IO.File.WriteAllBytes(filePath, fileData);

				var responseData = new {
					id = 0,
					fileName = fileName,
					folderName = folderName,
					operatorName = string.Format("{0} {1}", QuestionnaireContext.Current.User.FirstName, QuestionnaireContext.Current.User.LastName)
				};

				return JsonResult(responseData);
			}
			return JsonResult(false);
		}

		[Route("RemoveFile/{id}/{folderName}/{isTemporary}"), HttpGet]
		public ActionResult RemoveFile(int id, string folderName, bool isTemporary) {
			if(isTemporary) {
				var folderPath = Path.Combine("~/Uploads/TemporaryUploads", folderName);
				var folderPhisicalPath = UtilExtenders.MapPath(folderPath);
				if(Directory.Exists(folderPhisicalPath)) {
					Directory.Delete(folderPhisicalPath, true);
					return JsonResult();
				}
				return JsonResult(false);
			} else {
				_chatService.RemoveFile(id, folderName);
				return JsonResult();
			}
		}

		[Route("InitializeDownload/{id}/{folderName}/{isTemporary}"), HttpGet]
		public ActionResult InitializeDownload(int id, string folderName, bool isTemporary) {

			var folderPath = isTemporary ?
															Path.Combine("~/Uploads/TemporaryUploads", folderName) :
															Path.Combine("~/Uploads/ChatFiles", id.ToString(), folderName);

			var folderPhisicalPath = UtilExtenders.MapPath(folderPath);

			if(System.IO.Directory.Exists(folderPhisicalPath) && new DirectoryInfo(folderPhisicalPath).GetFiles().Count() > 0) {
				return Json(new { success = true }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { success = false }, JsonRequestBehavior.AllowGet);
		}

		[Route("Download/{id}/{folderName}/{isTemporary}"), HttpGet]
		public ActionResult Download(int id, string folderName, bool isTemporary) {
			var folderPath = isTemporary ?
														Path.Combine("~/Uploads/TemporaryUploads", folderName) :
														Path.Combine("~/Uploads/ChatFiles", id.ToString(), folderName);

			var folderPhisicalPath = UtilExtenders.MapPath(folderPath);

			if(System.IO.Directory.Exists(folderPhisicalPath)) {
				var _files = new DirectoryInfo(folderPhisicalPath).GetFiles();
				var _returnFile = _files.FirstOrDefault();
				var _file = System.IO.File.ReadAllBytes(_returnFile.FullName);
				return File(_file, System.Net.Mime.MediaTypeNames.Application.Octet, _returnFile.Name);
			}

			return null;
		}
	}
}