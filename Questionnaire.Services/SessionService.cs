using Questionnaire.Domain;
using Questionnaire.Domain.Entities;
using Questionnaire.Domain.Interfaces.Repositories;
using Questionnaire.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Services {
	public class SessionService : ISessionService {
		private IUserRepository _userRepository;
		private ICacheRepository _cacheRepository;

		public SessionService(IUserRepository userRepository, ICacheRepository cacheRepository) {
			_userRepository = userRepository;
			_cacheRepository = cacheRepository;
		}

		public bool Login(string username, string password) {
			var user = _userRepository.Fetch(username, password);
			if(user != null) {
				_cacheRepository.Save(User.CacheKey + user.Id, user); //TODO: We don't have user database on website side, because of this we store user in cache
				QuestionnaireContext.Current.User = user;
				if(QuestionnaireContext.Current.IsInRole("admin")) {
					QuestionnaireContext.Current.PreActiveIsChecked = true; // PreActive should be turned on for admin
				}

				QuestionnaireContext.Save();
				return QuestionnaireContext.Current.User != null;
			}
			return false;
		}

		public void LogOut() {
			_cacheRepository.RemoveUserCaches();
			_cacheRepository.Remove(User.CacheKey + QuestionnaireContext.Current.UserId);
			QuestionnaireContext.Current.PreActiveIsChecked = false;
			QuestionnaireContext.Current.User = null;
			QuestionnaireContext.Save();
		}
	}
}