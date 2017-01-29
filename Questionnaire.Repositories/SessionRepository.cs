using Questionnaire.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Domain;

namespace Questionnaire.Repositories {
	public class SessionRepository : ISessionRepository {
		private ICacheRepository _cacheRepository;
		private OracleContext _context;

		public SessionRepository(OracleContext context, ICacheRepository cacheRepository) {
			_cacheRepository = cacheRepository;
			_context = context;
		}

		public UserSession Fetch(string token) {
			if(string.IsNullOrEmpty(token)) {
				return new UserSession {
					Token = UserSession.NewToken,
					UserId = null,
					Username = string.Empty,
					CreateDate = DateTime.UtcNow,
					LastAccess = DateTime.UtcNow,
					LanguageCode = "en-US",
					TimeZoneOffSet = 0
				};
			}
			var session = _cacheRepository.Fetch(token);
			if(session == null) {
				return new UserSession {
					Token = UserSession.NewToken,
					UserId = null,
					Username = string.Empty,
					CreateDate = DateTime.UtcNow,
					LastAccess = DateTime.UtcNow,
					LanguageCode = "en-US",
					TimeZoneOffSet = 0
				};
			}
			return session as UserSession;
		}

		public void Save(UserSession session) {
			var _token = _cacheRepository.Fetch(session.Token);
			_cacheRepository.Save(session.Token, session);
		}

		public void LogOut(int userId) {
			//TODO: Needs to be implemented
		}
	}
}