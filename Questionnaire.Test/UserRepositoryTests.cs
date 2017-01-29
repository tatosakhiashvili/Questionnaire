using Microsoft.VisualStudio.TestTools.UnitTesting;
using Questionnaire.Domain.Interfaces.Repositories;
using Questionnaire.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Test {
	[TestClass]
	public class UserRepositoryTests : TestBase {
		private IUserRepository _userRepository;

		public UserRepositoryTests() {
			_userRepository = Resolve<IUserRepository>();
		}

		[TestMethod]
		public void Login() {
			var user = _userRepository.Fetch("admin", "admin");
			Assert.IsNotNull(user);
			Assert.IsTrue(user.Id > 0);
		}

		[TestMethod]
		public void Logout() {

		}
	}
}
