using Questionnaire.Domain.Interfaces.Services;
using Questionnaire.Domain.Utils;
using Questionnaire.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Questionnaire.Controllers {
	[RoutePrefix("RoamingPrice")]
	public class RoamingPriceController : BaseAuthorizedController {

		private IRoamingService _roamingService;

		public RoamingPriceController(IRoamingService roamingService) {
			_roamingService = roamingService;
		}

		[Route(""), HttpGet]
		public ActionResult Index() {
			var model = new RoamingViewModel { Countries = new List<RoamingCountryViewModel> { } };
			var roamingCountries = _roamingService.GetCountries();
			model.Countries = roamingCountries.Select(x => (RoamingCountryViewModel)x).ToList();
			return View(model);
		}

		[Route("GetRoamingOperators/{countryId}"), HttpGet]
		public ActionResult GetRoamingOperators(int countryId) {
			var model = new OperatorsViewModel { Operators = new List<OperatorViewModel> { } };
			var operators = _roamingService.GetOperators(countryId);
			model.Operators = operators.Select(x => (OperatorViewModel)x).ToList();
			return View(model);
		}

		[Route("GetRoamingTariffs/{operatorId}"), HttpGet]
		public ActionResult GetRoamingTariffs(int operatorId) {
			var tariffs = _roamingService.GetTariffs(operatorId);
			var tariff = tariffs.FirstOrDefault();
			var model = new TariffViewModel {
				OperatorId = operatorId,
				EmailSubject = tariff.EmailSubject,
				HtmlContent = string.Empty
			};

			var template = GetRoamingPriceTemplateFile();
			model.HtmlContent = FillRoamingTemplate(template, tariff.TariffParameters);

			return View(model);
		}

		[Route("SendRoamingPriceNotification"), HttpPost, ValidateInput(false)]
		public ActionResult SendRoamingPriceNotification(SendRoamingPriceNotificationViewModel model) {
			var template = GetRoamingPriceTemplateFile();
			if(!string.IsNullOrEmpty(template)) {
				var tariffs = _roamingService.GetTariffs(model.OperatorId);
				if(tariffs.Count() > 0) {
					var tariff = tariffs.FirstOrDefault();
					var emailBody = FillRoamingTemplate(template, tariff.TariffParameters);
					var result = _roamingService.SendRoamingTariffNotification(model.Email, model.EmailSubject, emailBody);
					return JsonResult(result);
				} else {
					return JsonResult(false);
				}
			} else {
				return JsonResult(false);
			}
		}

		private string GetRoamingPriceTemplateFile() {
			var RoamingTemplateFolder = UtilExtenders.MapPath("~/Uploads/Roaming");
			var RoamingTemplateFiles = UtilExtenders.GetFilesInDirectory(RoamingTemplateFolder);
			if(RoamingTemplateFiles.Count > 0) {
				var templateFile = RoamingTemplateFiles.FirstOrDefault();
				return System.IO.File.ReadAllText(templateFile);
			} else {
				return string.Empty;
			}
		}

		private string FillRoamingTemplate(string template, Dictionary<string, object> keyPairs) {
			foreach(var item in keyPairs) {
				template = template.Replace(string.Format("@{0}@", item.Key), string.IsNullOrEmpty(item.Value.ToString()) ? "-" : item.Value.ToString());
			}
			return template;
		}

	}
}