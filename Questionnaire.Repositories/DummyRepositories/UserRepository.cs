using Questionnaire.Domain.Entities;
using Questionnaire.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Repositories.DummyRepositories {
	public class UserRepository : IUserRepository {
		private ICacheRepository _cacheRepository;
		private OracleContext _context;

		public UserRepository(ICacheRepository cacheRepository, OracleContext context) {
			_cacheRepository = cacheRepository;
			_context = context;
		}

		public User Fetch(string username, string password) {			
			if(username != "admin" && password != "admin") {
				var user = new User {
					FirstName = "admin",
					LastName = "admin",
					Id = 183,
					GroupId = 122,
					GroupName = "---",
					Username = "---",
					Roles = new List<Role> { }
				};

				if(user.GroupId == 122) {
					user.Roles.Add(new Role { Id = 1, Name = "Admin" });
				}

				return user;
			}

			return null;
		}
	}
}
