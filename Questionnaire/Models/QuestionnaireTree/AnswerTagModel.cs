using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Models {
	public class AnswerTagModel {
		public int NodeId { get; set; }
		public List<AnswerTagTreeModel> TreeNodes { get; set; }
	}

	public class AnswerTagTreeModel {
		public int Id { get; set; }
		public string Text { get; set; }
	}
}
