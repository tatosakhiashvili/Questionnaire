using Questionnaire.Infrastructure.Attributes;
using Questionnaire.Infrastructure.Utils;
using Questionnaire.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Questionnaire.Controllers {
	[RoutePrefix("Diff")]
	public class DiffController : BaseAuthorizedController {
		public ActionResult Index() {
			return View();
		}

		[Route("GetDiff"), HttpGet]
		public ActionResult GetDiff(string oldText, string newText) {
			DifferenceFinderModel model = DifferenceFinder.GetDifferences(oldText, newText);
			return View(model);
		}

		[Route("DiffPanel"), HttpPost]
		public ActionResult DiffPanel(DiffPaneModelParser model) {
			return View(model);
		}
	}
}