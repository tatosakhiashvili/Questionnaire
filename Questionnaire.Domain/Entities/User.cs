using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Domain.Entities {
	public class User {
		public decimal Id { get; set; }
		public string Username { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public decimal GroupId { get; set; }
		public string GroupName { get; set; }
		public List<Role> Roles { get; set; }
		public static string CacheKey {
			get {
				return "Questionnaire_User_Cache_";
			}
		}
	}
}
