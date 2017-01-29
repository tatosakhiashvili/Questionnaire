using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Domain {
	public class DaemonMessage {
		public int Id { get; set; }
		public int MessageType { get; set; }
		public string Receiver { get; set; }
		public string MessageText { get; set; }
		public string Subject { get; set; }
		public int SendHour { get; set; }
		public DateTime SendDate { get; set; }
	}
}
