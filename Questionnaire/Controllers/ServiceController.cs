using Questionnaire.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Questionnaire.Controllers {
	[RoutePrefix("Service")]
	public class ServiceController : BaseController {

		private ICacheRepository _cacheRepository;

		public ServiceController(ICacheRepository cacheRepository) {
			_cacheRepository = cacheRepository;
		}

		public ActionResult Index() {
			return View();
		}

		[Route("AddMobileNumber/{userId}/{mobileNumber}"), HttpGet]
		public ActionResult AddMobileNumber(decimal userId, string mobileNumber) {
			var mobileCalls = new Dictionary<decimal, string>();
			var cacheMobileCallsKey = "cacheMobileCallKey";
			var cacheMinutesToStoreNumber = 10;
			var cachedMobileCalls = _cacheRepository.Fetch(cacheMobileCallsKey);

			if(cachedMobileCalls != null && cachedMobileCalls is Dictionary<decimal, string>) {
				var userCacheCalls = cachedMobileCalls as Dictionary<decimal, string>;
				if(userCacheCalls.ContainsKey(userId)) {
					userCacheCalls[userId] = mobileNumber;
				} else {
					userCacheCalls.Add(userId, mobileNumber);
					_cacheRepository.Save(cacheMinutesToStoreNumber, cacheMobileCallsKey, userCacheCalls);
				}
			} else {
				var cacheList = new Dictionary<decimal, string> { };
				cacheList.Add(userId, mobileNumber);
				_cacheRepository.Save(cacheMinutesToStoreNumber, cacheMobileCallsKey, cacheList);
			}

			return new HttpStatusCodeResult(HttpStatusCode.OK);
		}
	}
}