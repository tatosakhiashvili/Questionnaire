using Questionnaire.Configuration;
using Questionnaire.Controllers;
using Questionnaire.Domain;
using Questionnaire.Domain.Interfaces.Repositories;
using Questionnaire.Domain.Interfaces.Services;
using Questionnaire.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Questionnaire.Infrastructure.Attributes {
	public class InitQuestionnaireContextAttribute : ActionFilterAttribute {

		protected BaseController GetController(ActionExecutingContext actionContext) {
			var controller = actionContext.Controller.ControllerContext.Controller as BaseController;
			if(controller == null) throw new InvalidOperationException("GetController");
			return controller;
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext) {
			QuestionnaireContext.Initialize(
							DependencyResolver.Current.GetService<ISessionRepository>(),
							DependencyResolver.Current.GetService<IUserService>(),
							DependencyResolver.Current.GetService<Settings>(),
														key => {
															switch(key) {
																case QuestionnaireContext.TOKEN_KEY:
																	return QuestionnaireBearerManager.GetValue(key);
																default:
																	return QuestionnaireBearerManager.GetValue(key);
															}
														});
		}

		public override void OnActionExecuted(ActionExecutedContext filterContext) {
			QuestionnaireContext.Save(
					(key, value) => QuestionnaireBearerManager.SetValue(key, (value ?? string.Empty).ToString())
			);
		}
	}

	public class InitQuestionnaireAuthorizeAttribute : InitQuestionnaireContextAttribute {
		//private UserPermission[] _roles;

		public InitQuestionnaireAuthorizeAttribute() : base() { }

		//public InitQuestionnaireAuthorizeAttribute(params UserPermission[] roles) : base() {
		//	_roles = roles;
		//}

		public override void OnActionExecuting(ActionExecutingContext filterContext) {
			base.OnActionExecuting(filterContext);

			if(QuestionnaireContext.Current == null || !QuestionnaireContext.Current.IsAuthenticated) {
				filterContext.Result = GetController(filterContext).RequireAuthorize();
				return;
			}

			//if(_roles != null) {
			//	foreach(var role in _roles) {
			//		if(JamContext.Current.IsInRole(role))
			//			return;
			//	}

			//	filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
			//}
		}
	}
}