using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Domain.Entities {
	public class InternalMessage {
		public int Id { get; set; }
		public string Text { get; set; }
		public string Comment { get; set; }
		public string Owner { get; set; }
		public string Operator { get; set; }
		public string Status { get; set; }
		public int StatusId { get; set; }
		public string Priority { get; set; }
		public int TypeId { get; set; }
		public int PriorityId { get; set; }
		public DateTime CreateDate { get; set; }
		public DateTime ProcessDate { get; set; }
		public DateTime LastUpdateDate { get; set; }
	}

	public class InternalMessageFile {
		public int Id { get; set; }
		public int MessageId { get; set; }
		public string Name { get; set; }
		public string Operator { get; set; }
		public DateTime CreateDate { get; set; }
	}

	public class InternalMessageStatus {
		public int Id { get; set; }
		public string Name { get; set; }
		public string Operator { get; set; }
		public DateTime CreateDate { get; set; }
	}
}
