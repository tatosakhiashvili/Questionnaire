using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Domain.Interfaces.Repositories {
	public interface IRoamingRepository {
		IEnumerable<Country> GetCountries(decimal userId, int languageId);
		IEnumerable<Operator> GetOperators(int countryId, decimal userId, int languageId);
		IEnumerable<Tariff> GetTariffs(int operatorId, decimal userId, int languageId);
	}
}
