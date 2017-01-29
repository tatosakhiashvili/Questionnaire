using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Domain {
	public class Tree {
		public int Id { get; set; }
		public int ParentId { get; set; }
		public string FullName { get; set; }
		public string UserName { get; set; }
		public string Text { get; set; }
		public string Comment { get; set; }
		public string Status { get; set; }
		public int StatusId { get; set; }
		public string Color { get; set; }
		public bool IsBold { get; set; }
		public DateTime FromDate { get; set; }
		public DateTime ToDate { get; set; }
		public DateTime? CreateDate { get; set; }
		public int? NewsDays { get; set; }
		public int RootId { get; set; }
		public static string CacheKey {
			get {
				return "Questionnaire_Tree_Cache_";
			}
		}
	}

	public class DeletedTree {
		public int Id { get; set; }
		public string Caption { get; set; }
		public DateTime FromDate { get; set; }
		public DateTime ToDate { get; set; }
		public string Username { get; set; }
		public string Comments { get; set; }
		public bool IsNew { get; set; }
		public string DeleteType { get; set; }
	}
}
