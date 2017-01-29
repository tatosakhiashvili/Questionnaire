using Questionnaire.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Domain.Interfaces.Repositories {
	public interface IInternationalCodeRepository {
		IEnumerable<Code> GetInternationalCodes(decimal userId, int languageId);
	}
}
