using Questionnaire.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Questionnaire.Models {
	public class ReminderItemsViewModel {
		public List<ReminderItemViewModel> Records { get; set; }
	}

	public class ReminderItemViewModel {
		public int Id { get; set; }
		public string Caption { get; set; }
		public string FromDate { get; set; }
		public string ToDate { get; set; }
		public string InDate { get; set; }
		public string User { get; set; }
		public string Comments { get; set; }
		public decimal Remind { get; set; }

		public static implicit operator ReminderItemViewModel(Reminder reminder) {
			return new ReminderItemViewModel {
				Id = reminder.Id,
				Caption = reminder.Caption,
				FromDate = reminder.FromDate.ToString("dd/MM/yyyy hh:mm:ss"),
				ToDate = reminder.ToDate.ToString("dd/MM/yyyy hh:mm:ss"),
				InDate = reminder.InDate.ToString("dd/MM/yyyy hh:mm:ss"),
				User = reminder.User,
				Comments = reminder.Comments,
				Remind = reminder.Remind
			};
		}
	}
}