using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Models {
	public class MaintainMessageViewModel {
		public int Id { get; set; }
		public string Text { get; set; }
		public string Comment { get; set; }
		public MessageModifyStatus ModifyStatus { get; set; }
		public int Type { get; set; }
		public int Priority { get; set; }
		public double? ProcessDateEpoch { get; set; }
		public List<DropDownViewModel> Types { get; set; }
		public List<DropDownViewModel> Priorities { get; set; }
		public List<ChatItemFileViewModel> Files { get; set; }
	}

	public enum MessageModifyStatus {
		Add = 1,
		Change = 2,
		Execute = 3,
		Delete = 4
	}
}
