using Questionnaire.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Models {
	public class SmsNotificationControlView {
		public int TreeId { get; set; }
		public List<SmsNotificationGroup> Groups { get; set; }

		public static implicit operator SmsNotificationControlView(List<Sms> smsNotifications) {
			var model = new SmsNotificationControlView { Groups = new List<SmsNotificationGroup> { } };
			var groupedSmsNotifications = smsNotifications.GroupBy(x => x.GroupName);
			foreach(var item in groupedSmsNotifications) {
				model.Groups.Add(new SmsNotificationGroup {
					GroupName = item.FirstOrDefault().GroupName,
					SortOrder = item.FirstOrDefault().SortOrder,
					Comment = item.FirstOrDefault().Comment,
					SmsNotifications = item.Select(x => (SmsNotification)x).ToList()
				});
			}
			return model;
		}
	}

	public class SmsNotificationGroup {
		public string GroupName { get; set; }
		public int? SortOrder { get; set; }
		public string Comment { get; set; }
		public List<SmsNotification> SmsNotifications { get; set; }
	}

	public class SmsNotification {
		public int Id { get; set; }
		public string Text { get; set; }
		public static implicit operator SmsNotification(Sms sms) {
			return new SmsNotification {
				Id = sms.Id,
				Text = sms.Text
			};
		}
	}
}
