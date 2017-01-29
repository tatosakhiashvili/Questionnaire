using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Models {

	public class CurrentCallMobileNumberViewModel {
		public int LoadIntervalSeconds { get; set; }
	}

	public class CurrentCallMobileNumberModel {
		public string MobileNumber { get; set; }
		public bool ChangeMobileInput { get; set; }
	}
}
