using Questionnaire.Infrastructure.Attributes;
using Questionnaire.Models;
using Questionnaire.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Questionnaire.Controllers {
	[RoutePrefix("InternationalPrice")]
	public class InternationalPriceController : BaseAuthorizedController {

		private InternationalPriceService _internationalPriceService;

		public InternationalPriceController(InternationalPriceService internationalPriceService) {
			_internationalPriceService = internationalPriceService;
		}

		[Route(""), HttpGet]
		public ActionResult Index() {
			var countries = _internationalPriceService.GetCountries().Select(x => (InternationalPriceCountryViewModel)x).ToList();
			var companies = _internationalPriceService.GetCompanies().Select(x => (InternationalPriceCompanyViewModel)x).ToList();

			var model = new InternationalPriceViewModel {
				Countries = countries,
				Companies = companies
			};

			return View(model);
		}

		[Route("GetInternationalTariffs"), HttpPost, ValidateInput(false)]
		public ActionResult GetInternationalTariffs(InternationalPriceGetTariffsViewModel model) {
			//int countryCode, int companyCode
			var internationalTariffs = _internationalPriceService.GetInternationalTariffrs(model.CountryCode, model.CompanyCode).Select(x => (InternationalTariffViewModel)x).ToList();
			var result = new InternationalTariffsViewModel {
				Records = internationalTariffs
			};
			return View(result);
		}
	}
}