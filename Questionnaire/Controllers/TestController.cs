using Questionnaire.Domain.Interfaces.Services;
using Questionnaire.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Questionnaire.Controllers {

	[RoutePrefix("Test")]
	public class TestController : BaseAuthorizedController {

		private IQuestionnaireService _questionnaireService;

		public TestController(IQuestionnaireService questionnaireService) {
			_questionnaireService = questionnaireService;
		}

		[Route("Index"), HttpGet]
		public ActionResult Index() {

			var _date = new DateTime(1900, 1, 1, 0, 0, 0, 0);
			var _treeItems = _questionnaireService.GetTree(_date, true, true).ToList();
			
			var _treeItemsModel = QuestionnaireTreeViewModel.ParseFrom(_treeItems);
			var _expanded = QuesionnaireExpandedViewModel.ExpandTree(_treeItemsModel);
			
			return View(_treeItemsModel);
		}

		[Route("TreeItemControl/{item}"), HttpGet]
		public ActionResult TreeItemControl(QuestionnaireTreeViewModel item) {
			return View(item);
		}
	}
}