using Questionnaire.Domain;
using Questionnaire.Domain.Utils;
using Questionnaire.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Questionnaire.Controllers {

	[RoutePrefix("Upload")]
	public class UploadController : BaseAuthorizedController {

		public UploadController() {

		}

		[Route("Upload"), HttpPost]
		public ActionResult Upload() {
			if(Request.Files.Count > 0) {
				var file = Request.Files[0];
				var fileName = file.FileName;
				var folderName = Guid.NewGuid().ToString("N");

				var uploadPath = UtilExtenders.MapPath("~/Uploads/TemporaryUploads/" + folderName);
				var filePath = System.IO.Path.Combine(uploadPath, fileName);

				var fileData = GetFileData(file);

				System.IO.File.WriteAllBytes(filePath, fileData);

				var responseData = new {
					fileName = fileName,
					folderName = folderName
				};

				return Json(new { success = true, data = responseData }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { success = false }, JsonRequestBehavior.AllowGet);
		}

		[Route("UploadTreeFile"), HttpPost]
		public ActionResult UploadTreeFile() {
			if(Request.Files.Count > 0) {
				var file = Request.Files[0];
				var fileName = file.FileName;
				var folderName = Guid.NewGuid().ToString("N");

				var uploadPath = UtilExtenders.MapPath("~/Uploads/TemporaryUploads/" + folderName);
				var filePath = System.IO.Path.Combine(uploadPath, fileName);

				var fileData = GetFileData(file);

				System.IO.File.WriteAllBytes(filePath, fileData);

				var responseData = new {
					fileName = fileName,
					folderName = folderName
				};
				return JsonResult(responseData);
			}
			return JsonResult(false);
		}

		[Route("Download/{rootId}/{folderName}/{fileName}/{isTemporary}"), HttpGet]
		public ActionResult Download(int rootId, string folderName, string fileName, bool isTemporary) {
			var folderPath = isTemporary ?
														Path.Combine("~/Uploads/TemporaryUploads", folderName) :
														Path.Combine("~/Uploads/EmailAnswerFiles", rootId.ToString(), folderName);

			var filePath = Path.Combine(folderPath, fileName);

			var folderPhisicalPath = UtilExtenders.MapPath(folderPath);
			var filePhisicalPath = Path.Combine(folderPhisicalPath, fileName);

			if(System.IO.Directory.Exists(folderPhisicalPath) && System.IO.File.Exists(filePhisicalPath)) {
				var _file = System.IO.File.ReadAllBytes(filePhisicalPath);
				return File(_file, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
			}

			return null;
		}

		[Route("InitializeDownload/{rootId}/{folderName}/{fileName}/{isTemporary}"), HttpGet]
		public ActionResult InitializeDownload(int rootId, string folderName, string fileName, bool isTemporary) {

			var folderPath = isTemporary ?
															Path.Combine("~/Uploads/TemporaryUploads", folderName) :
															Path.Combine("~/Uploads/EmailAnswerFiles", rootId.ToString(), folderName);

			var filePath = Path.Combine(folderPath, fileName);

			var folderPhisicalPath = UtilExtenders.MapPath(folderPath);
			var filePhisicalPath = Path.Combine(folderPhisicalPath, fileName);

			if(System.IO.Directory.Exists(folderPhisicalPath) && System.IO.File.Exists(filePhisicalPath)) {
				return Json(new { success = true }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { success = false }, JsonRequestBehavior.AllowGet);
		}

		[Route("InitializeTreeFileDownload/{rootId}/{folderName}/{fileName}/{isTemporary}"), HttpGet]
		public ActionResult InitializeTreeFileDownload(int rootId, string folderName, string fileName, bool isTemporary) {

			var folderPath = isTemporary ?
															Path.Combine("~/Uploads/TemporaryUploads", folderName) :
															Path.Combine("~/Uploads/TreeFiles", rootId.ToString(), folderName);

			var filePath = Path.Combine(folderPath, fileName);

			var folderPhisicalPath = UtilExtenders.MapPath(folderPath);
			var filePhisicalPath = Path.Combine(folderPhisicalPath, fileName);

			if(System.IO.Directory.Exists(folderPhisicalPath) && System.IO.File.Exists(filePhisicalPath)) {
				return Json(new { success = true }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { success = false }, JsonRequestBehavior.AllowGet);
		}

		[Route("DownloadTreeFile/{rootId}/{folderName}/{fileName}/{isTemporary}"), HttpGet]
		public ActionResult DownloadTreeFile(int rootId, string folderName, string fileName, bool isTemporary) {
			var folderPath = isTemporary ?
														Path.Combine("~/Uploads/TemporaryUploads", folderName) :
														Path.Combine("~/Uploads/TreeFiles", rootId.ToString(), folderName);

			var filePath = Path.Combine(folderPath, fileName);

			var folderPhisicalPath = UtilExtenders.MapPath(folderPath);
			var filePhisicalPath = Path.Combine(folderPhisicalPath, fileName);

			if(System.IO.File.Exists(filePhisicalPath)) {
				var _file = System.IO.File.ReadAllBytes(filePhisicalPath);
				return File(_file, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
			}

			return null;
		}

		[Route("UploadDialogFile"), HttpPost]
		public ActionResult UploadDialogFile(GetFileUploadDialogModel model) {
			if(Request.Files.Count > 0) {
				var file = Request.Files[0];
				var fileName = file.FileName;
				var folderName = Guid.NewGuid().ToString("N");

				var uploadPath = UtilExtenders.MapPath("~/Uploads/TemporaryUploads/" + folderName);
				var filePath = System.IO.Path.Combine(uploadPath, fileName);

				var fileData = GetFileData(file);

				System.IO.File.WriteAllBytes(filePath, fileData);

				var responseData = new {
					fileName = fileName,
					folderName = folderName,
					comment = model.Comment
				};

				return Json(new { success = true, data = responseData }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { success = false }, JsonRequestBehavior.AllowGet);
		}

		[Route("UploadAnswerDialogFile"), HttpPost]
		public ActionResult UploadAnswerDialogFile(GetTreeFileUploadDialogModel model) {
			if(Request.Files.Count > 0) {
				var file = Request.Files[0];
				var fileName = file.FileName;
				var folderName = Guid.NewGuid().ToString("N");

				var uploadPath = UtilExtenders.MapPath("~/Uploads/TemporaryUploads/" + folderName);
				var filePath = System.IO.Path.Combine(uploadPath, fileName);

				var fileData = GetFileData(file);

				System.IO.File.WriteAllBytes(filePath, fileData);

				var responseData = new {
					fileName = fileName,
					folderName = folderName,
					comment = model.Comment
				};

				return Json(new { success = true, data = responseData }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { success = false }, JsonRequestBehavior.AllowGet);
		}
	}
}