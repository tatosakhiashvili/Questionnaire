using Questionnaire.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Domain.Entities;

namespace Questionnaire.Repositories.DummyRepositories {
	public class InternationalCodeRepository : IInternationalCodeRepository {
		public IEnumerable<Code> GetInternationalCodes(decimal userId, int languageId) {
			return new List<Code> {
				new Code { Cc = "cc", CityNameEng = "City Name eng", CityNameGeo = "City Name Geo", NameEng = "Name Eng", NameGeo = "Name Geo", SmsText = "Sms Text", SmsTextCity = "Sms Text City", TelCode = "Tel Code" },
				new Code { Cc = "cc 1", CityNameEng = "City Name eng 1", CityNameGeo = "City Name Geo 1", NameEng = "Name Eng 1", NameGeo = "Name Geo 1", SmsText = "Sms Text 1", SmsTextCity = "Sms Text City 1", TelCode = "Tel Code 1" },
			};
		}
	}
}
