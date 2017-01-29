using Questionnaire.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Services {
	public class DateService : IDateService {
		public DateTime GetEpochDate() {
			return new DateTime(1970, 1, 1, 0, 0, 0, 0);
		}
	}
}
