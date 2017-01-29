using Questionnaire.Domain.Interfaces.Services;
using Questionnaire.Infrastructure.Attributes;
using Questionnaire.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Questionnaire.Controllers {
	[RoutePrefix("Account")]
	[InitQuestionnaireContext]
	public class AccountController : BaseController {
		private ISessionService _sessionService;

		public AccountController(ISessionService sessionService) {
			_sessionService = sessionService;
		}

		[Route("Login"), HttpGet]
		public ActionResult LogIn() {
			return View(new LoginModel { });
		}

		[Route("Login"), HttpPost]
		public ActionResult LogIn(LoginModel model) {
			var loginResponse = _sessionService.Login(model.Username, model.Password);
			if(!loginResponse) {
				model.ErrorMessage = "არასწორი მომხმარებლის სახელი ან პაროლი";
				return View(model);
			}
			return Redirect("/");
		}

		[Route("LogOut"), HttpGet]
		public ActionResult LogOut() {
			_sessionService.LogOut();
			return Redirect("/");
		}
	}
}