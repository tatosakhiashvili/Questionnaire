using Questionnaire.Domain.Interfaces.Services;
using Questionnaire.Infrastructure.Attributes;
using Questionnaire.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Questionnaire.Controllers {
	[RoutePrefix("InternationalCode")]
	public class InternationalCodeController : BaseAuthorizedController {

		private IInternationalCodeService _internationalCodeService;

		public InternationalCodeController(IInternationalCodeService internationalCodeService) {
			_internationalCodeService = internationalCodeService;
		}

		[Route(""), HttpGet]
		public ActionResult Index() {
			var internationalCodes = _internationalCodeService.GetInternationalCodes().Select(x => (InternationalCodeViewModel)x).ToList();
			var model = new InternationalCodesViewModel {
				Codes = internationalCodes
			};
			return View(model);
		}
	}
}