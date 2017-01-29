using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Domain.Entities {
	public class EmailComparisonFile {
		public int Id { get; set; }
		public int TreeId { get; set; }
		public string FolderName { get; set; }
		public string Comment { get; set; }
	}
}
