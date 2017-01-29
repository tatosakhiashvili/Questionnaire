using Questionnaire.Domain;
using Questionnaire.Domain.Interfaces.Repositories;
using Questionnaire.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Domain.Entities;

namespace Questionnaire.Services {
	public class UserService : IUserService {
		private IUserRepository _userRepository;
		private ICacheRepository _cacheRepository;

		public UserService(IUserRepository userRepository, ICacheRepository cacheRepository) {
			_userRepository = userRepository;
			_cacheRepository = cacheRepository;
		}

		public User Fetch(decimal id) {
			var user = _cacheRepository.Fetch(User.CacheKey + id);
			if(user != null && user is User) {
				return user as User;
			}
			return null;
		}
	}
}

