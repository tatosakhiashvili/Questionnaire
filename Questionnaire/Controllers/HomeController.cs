using Questionnaire.Domain;
using Questionnaire.Domain.Interfaces.Services;
using Questionnaire.Domain.Utils;
using Questionnaire.Infrastructure.Attributes;
using Questionnaire.Infrastructure.Utils;
using Questionnaire.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Questionnaire.Controllers {
	[RoutePrefix("")]
	public class HomeController : BaseAuthorizedController {
		private IQuestionnaireService _questionnaireService;

		public HomeController(IQuestionnaireService questionnaireService) {
			_questionnaireService = questionnaireService;
		}

		[Route(""), HttpGet]
		public ActionResult Index() {
			return View();
		}

		[Route("SmsTabControl/{treeId}/{isHistoryOn}/{historyDateEpoch}"), HttpGet]
		public ActionResult SmsTabControl(int treeId, bool isHistoryOn, double historyDateEpoch) {
			var historyDate = DateTime.Now;
			if(isHistoryOn) {
				if(historyDateEpoch > 0) {
					historyDate = historyDateEpoch.FromEpoch();
				}
			}
			var model = (SmsNotificationControlView)_questionnaireService.GetTreeSms(treeId, historyDate).ToList();
			model.TreeId = treeId;
			return View(model);
		}

		[Route("TopItemsControl"), HttpGet]
		public ActionResult TopItemsControl() {
			return View();
		}

		[Route("AddSms/{treeId}"), HttpGet]
		public ActionResult AddSms(int treeId) {
			var smsModel = (SmsNotificationControlView)_questionnaireService.GetTreeSms(treeId, DateTime.Now).ToList();
			var model = new AddSmsModel { TreeId = treeId, Groups = new List<SmsGroupModel> { } };
			foreach(var item in smsModel.Groups) {
				model.Groups.Add(new SmsGroupModel {
					Id = 1,
					Name = item.GroupName,
					SortOrder = item.SortOrder,
					Comment = item.Comment,
					SmsList = item.SmsNotifications.Select(x => x.Text).ToList(),
				});
			}
			return View(model);
		}

		[Route("AddSms/{treeId}"), HttpPost]
		public ActionResult AddSms(int treeId, AddSmsModel model) {
			try {
				var smsList = new List<string> { };
				if(model.SmsRawContent != null) {
					smsList = model.SmsRawContent.Split(new string[] { "\r\n" }, StringSplitOptions.None).ToList();
				}
				_questionnaireService.AddSmsToTree(treeId, model.GroupId, model.GroupName, model.GroupPreviousName, model.GroupSortOrder.Value, smsList, model.Comment);
				return Json(new { success = true }, JsonRequestBehavior.AllowGet);
			} catch(Exception ex) {
				return Json(new { success = false }, JsonRequestBehavior.AllowGet);
			}
		}

		[Route("SendSms/{number}"), HttpPost, ValidateInput(false)]
		public ActionResult SendSms(string number, List<string> smsList, int treeId) {

			if(!number.StartsWith("5") || number.Length != 9) {
				return JsonResult(false, "მობილურის ფორმატი არასწორია");
			}

			if(smsList.Count > 0) {
				var sendSmsResponse = _questionnaireService.SendSms(treeId, number, smsList.ToList());
				return JsonResult(sendSmsResponse, "SMS არ გაიგზავნა");
			}
			return JsonResult(false, "sms ტექსტი სავალდებულოა");
		}

		[Route("SendNotificationSms/{number}"), HttpPost, ValidateInput(false)]
		public ActionResult SendNotificationSms(string number, List<string> notificationsText) {
			var sendSmsResponse = _questionnaireService.SendSms(0, number, notificationsText);
			return JsonResult(sendSmsResponse, "SMS არ გაიგზავნა");
		}

		[Route("GetMsisdn/{number}"), HttpGet]
		public ActionResult GetMsisdn(string number) {
			var balance = AbonentBalanceLoader.GetBalance("", number);
			var emails = _questionnaireService.GetEmailGroups(number);

			return Json(new { success = true, balance = Math.Round(balance, 2), emails = emails }, JsonRequestBehavior.AllowGet);
		}

		[Route("RegisterMsisdn/{number}/{treeId}"), HttpGet]
		public ActionResult RegisterMsisdn(string number, int treeId) {
			var response = _questionnaireService.RegisterMsisdn(number, treeId);
			return JsonResult(response);
		}

		[Route("CurrentMobileNumber"), HttpGet]
		public ActionResult CurrentMobileNumber() {
			var model = new CurrentCallMobileNumberViewModel {
				LoadIntervalSeconds = QuestionnaireContext.Current.Settings.Questionnaire.CurrentMobileNumberLoadSeconds
			};
			return View(model);
		}

		[Route("GetCurrentCallMobileNumber"), HttpGet]
		public ActionResult GetCurrentCallMobileNumber() {
			var mobileNumber = _questionnaireService.GetCurrentCallMobileNumber();
			var currentCallModel = new CurrentCallMobileNumberModel { };

			if(!string.IsNullOrEmpty(mobileNumber) && QuestionnaireContext.Current.DbCallMobile != mobileNumber) {
				currentCallModel = new CurrentCallMobileNumberModel {
					MobileNumber = mobileNumber,
					ChangeMobileInput = QuestionnaireContext.Current.DbCallMobile != mobileNumber, //TODO: This needs to be implemented
				};
				QuestionnaireContext.Current.DbCallMobile = mobileNumber;
				QuestionnaireContext.Current.CallMobile = mobileNumber;
			}
			return JsonResult(currentCallModel);
		}

		[Route("UpdateSessionMobile"), HttpPost]
		public ActionResult UpdateSessionMobile(string mobileNumber) {
			QuestionnaireContext.Current.CallMobile = mobileNumber;
			return JsonResult();
		}
	}
}