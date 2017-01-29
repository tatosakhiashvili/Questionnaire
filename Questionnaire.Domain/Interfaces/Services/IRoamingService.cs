using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Domain.Interfaces.Services {
	public interface IRoamingService {
		IEnumerable<Country> GetCountries();
		IEnumerable<Operator> GetOperators(int countryId);
		IEnumerable<Tariff> GetTariffs(int operatorId);

		bool SendRoamingTariffNotification(string to, string title, string body);
	}
}
