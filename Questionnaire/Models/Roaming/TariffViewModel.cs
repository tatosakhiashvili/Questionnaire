using Questionnaire.Domain;
using Questionnaire.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Questionnaire.Models {
	public class TariffViewModel {
		public int OperatorId { get; set; }
		public string EmailSubject { get; set; }
		public string HtmlContent { get; set; }
	}
}