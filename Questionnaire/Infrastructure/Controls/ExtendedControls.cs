using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Questionnaire.Infrastructure.Controls {
	public static class ExtendedControls {
		public static MvcHtmlString DatePicker(this HtmlHelper helper, string id, object value, string requireMessage) {
			//var attributes = "";

			//foreach (PropertyInfo item in htmlAttributes.GetType().GetProperties())
			//{
			//    attributes += item.Name + ":'" + item.GetValue(htmlAttributes, null) + "';";
			//}

			//attributes.TrimEnd();

			var isRequired = !string.IsNullOrEmpty(requireMessage);
			var control = @"<div class='datepicker-container'>
                                <div class='input-group date' id=""{0}"">
                                    <input type='text' value=""{1}"" {2} {3} class='form-control' />
                                    <span class='input-group-addon'>
                                        <span class='glyphicon glyphicon-calendar'></span>
                                    </span>
                                </div>
                            </div>";

			control = string.Format(control, id, value, isRequired ? "required='required'" : "", isRequired ? "data-v-m:'" + requireMessage + "'" : "");

			control = control.Replace("'", "\"");

			return MvcHtmlString.Create(control);
		}

		public static MvcHtmlString DatePickerFor<M, P>(this HtmlHelper<M> helper, Expression<Func<M, P>> expression) {
			var member = (MemberExpression)expression.Body;
			var _name = member.Member.Name;

			var control = @"<div class='input-group date datepicker'> 
                                <input type='text' id="" value="" required data-v-m='საბოლოო თარიღი სავალდებულოა' class='form-control' />
                                <span class='input-group-addon'>
                                    <span class='glyphicon glyphicon-calendar'></span>
                                </span>
                            </div>
                            <input type='hidden' />";

			return MvcHtmlString.Create(control);
		}

		public static MvcHtmlString DatePickerFor<M, P>(this HtmlHelper<M> helper, Expression<Func<M, P>> expression, object htmlAttributes) {
			return null;
		}

		public static MvcHtmlString Table(this HtmlHelper helper, List<object> data) {
			var control = @"<table class='table table-hover favourite-list-body'>
													<tbody id='top-list-body-favourite-1' class='top-list-body-favourite-Selector' style='display: table-row-group;'>
															<tr class='favourite-remove' data-node-id='1' data-record-id='0'>
																<th scope='row'><a href='#'>Test 1</a></th>
															</tr>
															<tr class='favourite-remove' data-node-id='2' data-record-id='0'>
																<th scope='row'><a href='#'>Test 2</a></th>
															</tr>
															<tr class='favourite-remove' data-node-id='3' data-record-id='0'>
																<th scope='row'><a href='#'>Test 3</a></th>
															</tr>														
													</tbody>
											</table>";
			return MvcHtmlString.Create(control);
		}

		public static IHtmlString ParseToJson(this HtmlHelper helper, object obj) {
			var data = helper.Raw(HttpUtility.JavaScriptStringEncode(Newtonsoft.Json.JsonConvert.SerializeObject(obj)));
			return data;
		}

		public static MvcHtmlString Pager(this HtmlHelper helper, int pageNo, int pageCount) {
			var control = string.Empty;

			control += "<ul class='pagination table-pagination' data-current-page='" + pageNo + "' data-page-count='" + pageCount + "'>";
			control += "<li class='disabled'><a class='p-arrow p-previous' aria-label='Previous'><span aria-hidden='true'>&laquo;</span></a></li>";

			for(int i = 1; i <= pageCount; i++) {
				if(pageNo == i) {
					control += "<li class='active'><a>" + i + "</a></li>";
				} else {
					control += "<li><a>" + i + "</a></li>";
				}
			}

			control += "<li><a class='p-arrow p-next' aria-label='Next'><span aria-hidden='true'>&raquo;</span></a></li>";
			control += "</ul>";

			return MvcHtmlString.Create(control);
		}

		public static MvcHtmlString Searcher(this HtmlHelper helper, string table = "") {
			var control = string.Empty;

			control += @"<div class='row' style='float:right;'>
											<div class='col-lg-2'>ძებნა</div>
											<div class='col-lg-10'>
											 	<input type='text' data-table-id='" + table + "' class='form-control searcher-control' />";
			control += @"</div>
									 </div>
									 <div class='clearfix'></div>";

			return MvcHtmlString.Create(control);
		}
	}
}
