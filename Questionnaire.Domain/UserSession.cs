using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Domain {
	[Serializable]
	public class UserSession {
		public int Id { get; set; }
		public decimal? UserId { get; set; }
		public string Token { get; set; }
		public string Username { get; set; }
		public int TimeZoneOffSet { get; set; }
		public string LanguageCode { get; set; }
		public DateTime CreateDate { get; set; }
		public DateTime LastAccess { get; set; }
		public string CallMobile { get; set; }
		public int SelectedNodeId { get; set; }
		public string DbCallMobile { get; set; }
		public bool PreActiveIsSelected { get; set; }

		public static string NewToken {
			get { return string.Format("{0}.{1}.{2}", Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N")); }
		}
	}
}
