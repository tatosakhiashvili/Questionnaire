using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Questionnaire.Models {
	public class AddSmsModel {
		public int TreeId { get; set; }
		public int? GroupId { get; set; }
		public string GroupName { get; set; }
		public int? GroupSortOrder { get; set; }
		public string GroupPreviousName { get; set; }
		public string SmsRawContent { get; set; }
		public string Comment { get; set; }
		public List<SmsGroupModel> Groups { get; set; }
	}

	public class SmsGroupModel {
		public int Id { get; set; }
		public string Name { get; set; }
		public int? SortOrder { get; set; }
		public string Comment { get; set; }
		public List<string> SmsList { get; set; }
		public string RawSms {
			get {
				var result = string.Empty;
				foreach(var sms in SmsList) {
					result += sms + Environment.NewLine;
				}
				return result;
			}
		}
	}
}