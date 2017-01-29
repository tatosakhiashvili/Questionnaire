using Questionnaire.Domain.Interfaces.Services;
using Questionnaire.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Questionnaire.Controllers {
	[RoutePrefix("Store")]
	public class StoreController : BaseAuthorizedController {

		private IStoreService _storeService;

		public StoreController(IStoreService storeService) {
			_storeService = storeService;
		}

		[Route(""), HttpGet]
		public ActionResult Index() {
			var products = _storeService.GetProducts().Select(x => (ProductItemViewModel)x).ToList();
			var model = new ProductViewModel { Products = products };
			return View(model);
		}

		[Route("GetProductDetails/{productId}"), HttpGet]
		public ActionResult GetProductDetails(int productId) {
			var productDetails = _storeService.GetProductDetails(productId).Select(x => (ProductDetailItemViewModel)x).ToList();
			var model = new ProductDetailsViewModel {
				ProductDetails = productDetails
			};
			return View(model);
		}
	}
}