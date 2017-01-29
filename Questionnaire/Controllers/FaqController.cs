using Questionnaire.Domain;
using Questionnaire.Domain.Interfaces.Services;
using Questionnaire.Infrastructure.Attributes;
using Questionnaire.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Questionnaire.Controllers {
	[RoutePrefix("Faq")]
	public class FaqController : BaseAuthorizedController {
		private IFaqService _faqService;
		private IQuestionnaireService _questionnaireService;

		public FaqController(IFaqService faqService, IQuestionnaireService questionnaireService) {
			_faqService = faqService;
			_questionnaireService = questionnaireService;
		}

		public ActionResult Index() {
			return View();
		}

		[Route("FavouriteFaq"), HttpGet]
		public ActionResult FavouriteFaq() {
			var faqs = (FavouriteFaqModel)_faqService.GetFavouriteFaq().ToList();
			return View(faqs);
		}

		[Route("TopFaq/{topType}"), HttpGet]
		public ActionResult TopFaq(TopFaqEnumeration topType) {
			var faqs = (TopFaqModel)_faqService.GetTopFaq(topType).ToList();
			faqs.TopType = topType;
			return View(faqs);
		}

		[Route("CustomerFaq/{msisdn?}"), HttpGet]
		public ActionResult CustomerFaq(string msisdn) {
			var faqs = (CustomerFaqModel)_faqService.GetCustomerFaq(msisdn ?? "").ToList();
			return View(faqs);
		}
	}
}