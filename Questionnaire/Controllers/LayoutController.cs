using Questionnaire.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Questionnaire.Controllers
{
    [RoutePrefix("Layout")]
    public class LayoutController : BaseController
    {
        [Route("Header"), HttpGet]
        public ActionResult Header()
        {
            return View();
        }

        [Route("ErrorContainer"), HttpGet, ChildActionOnly]
        public ActionResult ErrorContainer()
        {
            return View();
        }

        [Route("Loader/{id}"), HttpGet]
        public ActionResult Loader(string id)
        {
            return View(new LoaderModel { LoaderId = id });
        }

        [Route("PaginationControl/{pageNo}/{totalPages}/{resultPerPage}"), HttpGet, ChildActionOnly]
        public ActionResult PaginationControl(int pageNo, int totalPages, int resultPerPage)
        {
            return View(new PaginationControlModel { PageNo = pageNo, TotalPages = totalPages, ResultPerPage = resultPerPage });
        }

        [Route("DatePicker/{id}/{value?}/{requiredMessage}"), HttpGet, ChildActionOnly]
        public ActionResult DatePicker(string id, DateTime value, string requiredMessage)
        {
            var model = new DatePickerModel { Id = id, Value = value, ErrorMessage = requiredMessage };
            return View(model);
        }
    }
}