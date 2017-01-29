using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Domain.Entities;
using Questionnaire.Domain;
using Questionnaire.Domain.Interfaces.Repositories;

namespace Questionnaire.Repositories.DummyRepositories {
	public class RoamingRepository : IRoamingRepository {

		public RoamingRepository() {

		}

		public IEnumerable<Country> GetCountries(decimal userId, int languageId) {
			return new List<Country> {
				new Country { Id = 1, Code = "-1-", NameEng = "Name 1", NameGeo = "სახელი 1" },
				new Country { Id = 1, Code = "-2-", NameEng = "Name 2", NameGeo = "სახელი 2" },
				new Country { Id = 1, Code = "-3-", NameEng = "Name 3", NameGeo = "სახელი 3" }
			};
		}

		public IEnumerable<Operator> GetOperators(int countryId, decimal userId, int languageId) {
			return new List<Operator> {
				new Operator { Id = 1, Name = "Test 1", CustomerCare = "Test 1", Display = "Test 1", GprsRoamingStatus = "Test 1", NetworkCode = "Test 1", NetworkType = "Test 1" },
				new Operator { Id = 2, Name = "Test 2", CustomerCare = "Test 2", Display = "Test 2", GprsRoamingStatus = "Test 2", NetworkCode = "Test 2", NetworkType = "Test 2" },
				new Operator { Id = 3, Name = "Test 3", CustomerCare = "Test 3", Display = "Test 3", GprsRoamingStatus = "Test 3", NetworkCode = "Test 3", NetworkType = "Test 3" },
			};
		}

		public IEnumerable<Tariff> GetTariffs(int operatorId, decimal userId, int languageId) {
			return new List<Tariff> {
				//new Tariff { CallSetup = "Test 1", CallSetupPeak = "Test 1", ChargingRule = "Test 1", ChargingRuleL = "Test 1", Comments = "Test 1", CommentsL = "Test 1", MocGeorgia = "Test 1", MocGeorgiaL = "Test 1", MocLocal = "Test 1", MocLocalL = "Test 1", MocLocalPeak = "Test 1", MocMms = "Test 1", MocMmsL = "Test 1", MocMmsPeak = "Test 1", MocOperator = "Test 1", MocOperatorL = "Test 1", MocOperatorPeak = "Test 1", MocSms = "Test 1", MocSmsL = "Test 1", MosGeorgiaPeak = "Test 1", MosGprs = "Test 1", MosGprsL = "Test 1", MosGprsPeak = "Test 1", MosSmsPeak = "Test 1", Mtc = "Test 1", MtcL = "Test 1", MtcPeak = "Test 1", PeakOffTime = "Test 1", PeakTime = "Test 1", TariffCurrency = "Test 1", TariffCurrencyL = "Test 1" },
				//new Tariff { CallSetup = "Test 2", CallSetupPeak = "Test 2", ChargingRule = "Test 2", ChargingRuleL = "Test 2", Comments = "Test 2", CommentsL = "Test 2", MocGeorgia = "Test 2", MocGeorgiaL = "Test 2", MocLocal = "Test 2", MocLocalL = "Test 2", MocLocalPeak = "Test 2", MocMms = "Test 2", MocMmsL = "Test 2", MocMmsPeak = "Test 2", MocOperator = "Test 2", MocOperatorL = "Test 2", MocOperatorPeak = "Test 2", MocSms = "Test 2", MocSmsL = "Test 2", MosGeorgiaPeak = "Test 2", MosGprs = "Test 2", MosGprsL = "Test 2", MosGprsPeak = "Test 2", MosSmsPeak = "Test 2", Mtc = "Test 2", MtcL = "Test 2", MtcPeak = "Test 2", PeakOffTime = "Test 2", PeakTime = "Test 2", TariffCurrency = "Test 2", TariffCurrencyL = "Test 2" },
				//new Tariff { CallSetup = "Test 3", CallSetupPeak = "Test 3", ChargingRule = "Test 3", ChargingRuleL = "Test 3", Comments = "Test 3", CommentsL = "Test 3", MocGeorgia = "Test 3", MocGeorgiaL = "Test 3", MocLocal = "Test 3", MocLocalL = "Test 3", MocLocalPeak = "Test 3", MocMms = "Test 3", MocMmsL = "Test 3", MocMmsPeak = "Test 3", MocOperator = "Test 3", MocOperatorL = "Test 3", MocOperatorPeak = "Test 3", MocSms = "Test 3", MocSmsL = "Test 3", MosGeorgiaPeak = "Test 3", MosGprs = "Test 3", MosGprsL = "Test 3", MosGprsPeak = "Test 3", MosSmsPeak = "Test 3", Mtc = "Test 3", MtcL = "Test 3", MtcPeak = "Test 3", PeakOffTime = "Test 3", PeakTime = "Test 3", TariffCurrency = "Test 3", TariffCurrencyL = "Test 3" },
			};
		}
	}
}
