using Questionnaire.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Questionnaire.Models {
	public class InternationalCodeViewModel {
		public string Cc { get; set; }
		public string NameGeo { get; set; }
		public string NameEng { get; set; }
		public string TelCode { get; set; }
		public string CityNameGeo { get; set; }
		public string CityNameEng { get; set; }
		public string SmsText { get; set; }
		public string SmsTextCity { get; set; }

		public static implicit operator InternationalCodeViewModel(Code code) {
			return new InternationalCodeViewModel {
				Cc = code.Cc,
				NameGeo = code.NameGeo,
				NameEng = code.NameEng,
				TelCode = code.TelCode,
				CityNameGeo = code.CityNameGeo,
				CityNameEng = code.CityNameEng,
				SmsText = code.SmsText,
				SmsTextCity = code.SmsTextCity
			};
		}
	}
}