using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Questionnaire.Infrastructure.Utils {
	internal static class QuestionnaireBearerManager {
		public static void SetValue(string name, string value) {
			HttpCookie Responce = new HttpCookie(name);
			Responce.Expires = DateTime.UtcNow.AddDays(2);
			Responce.Value = value;
			HttpContext.Current.Request.Cookies.Add(Responce);
			HttpContext.Current.Response.Cookies.Add(Responce);
		}

		public static string GetValue(string name) {
			var request = HttpContext.Current.Request;
			var stmCookie = request.Cookies[name];

			return stmCookie != null ?
					stmCookie.Value : null;
		}
	}
}