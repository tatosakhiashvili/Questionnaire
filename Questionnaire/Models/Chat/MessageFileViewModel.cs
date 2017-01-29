using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Models {
	public class MessageFileViewModel {
		public int MessageId { get; set; }
		public bool ShowControls { get; set; }
		public bool ShowSearchBox { get; set; }
		public List<ChatItemFileViewModel> Files { get; set; }
	}
}
