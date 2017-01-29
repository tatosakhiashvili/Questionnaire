using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Domain.Interfaces.Repositories {
	public interface ISessionRepository {
		UserSession Fetch(string token);
		void Save(UserSession session);
		void LogOut(int userId);
	}
}
