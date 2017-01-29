using Questionnaire.Infrastructure.Attributes;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Questionnaire.Controllers {
	public class BaseController : Controller {
		public ActionResult RequireAuthorize() {
			return RedirectToAction("Login", "Account", new { });
		}

		protected internal JsonResult JsonResult(bool success = true, string errorMessage = "ტექნიკური შეცდომა") {
			if(success) {
				return Json(new { success = success }, JsonRequestBehavior.AllowGet);
			} else {
				return Json(new { success = success, errorMessage = errorMessage }, JsonRequestBehavior.AllowGet);
			}
		}

		protected internal JsonResult JsonResult(object model, bool success = true, string errorMessage = "ტექნიკური შეცდომა") {
			if(success) {
				return Json(new { success = success, data = model }, JsonRequestBehavior.AllowGet);
			} else {
				return Json(new { success = success, errorMessage = errorMessage }, JsonRequestBehavior.AllowGet);
			}
		}

		public byte[] GetFileData(HttpPostedFileBase file) {
			byte[] fileData = null;
			using(var binaryReader = new BinaryReader(Request.Files[0].InputStream)) {
				fileData = binaryReader.ReadBytes(Request.Files[0].ContentLength);
			}
			return fileData;
		}
	}

	[InitQuestionnaireAuthorize]
	public class BaseAuthorizedController : BaseController {

	}
}