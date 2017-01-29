using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Questionnaire.Models {
	public class GetFileUploadDialogModel {
		public int TreeId { get; set; }
		public string Comment { get; set; }
	}

	public class GetTreeFileUploadDialogModel : GetFileUploadDialogModel {
		public int AnwerId { get; set; }
	}
}