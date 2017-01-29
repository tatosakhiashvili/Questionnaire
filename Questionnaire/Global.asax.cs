using Questionnaire.Domain;
using Questionnaire.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Questionnaire {
	public class MvcApplication : System.Web.HttpApplication {
		protected void Application_Start() {
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			UtilExtenders.MapPath = x => {
				string path = HttpContext.Current.Server.MapPath(x);

				if(!System.IO.Directory.Exists(path))
					System.IO.Directory.CreateDirectory(path);
				return path;
			};

			QuestionnaireContext.Storage = () => HttpContext.Current.Items;
		}
	}
}
