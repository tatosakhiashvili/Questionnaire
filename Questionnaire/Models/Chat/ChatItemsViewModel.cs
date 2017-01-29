using Questionnaire.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Models {
	public class ChatItemsViewModel {
		public List<ChatItemViewModel> Records { get; set; }
	}

	public class ChatItemViewModel {
		public int Id { get; set; }
		public string Text { get; set; }
		public string Comment { get; set; }
		public string Owner { get; set; }
		public string Operator { get; set; }
		public string Status { get; set; }
		public int StatusId { get; set; }
		public string Priority { get; set; }
		public string CreateDate { get; set; }
		public string ProcessDate { get; set; }
		public string LastUpdateDate { get; set; }

		public static implicit operator ChatItemViewModel(InternalMessage message) {

			return new ChatItemViewModel {
				Id = message.Id,
				Text = message.Text,
				Comment = message.Comment,
				Owner = message.Owner,
				Operator = message.Operator,
				Status = message.Status,
				StatusId = message.StatusId,
				Priority = message.Priority,
				CreateDate = message.CreateDate.ToString("dd/MM/yyyy HH:mm:ss"),
				ProcessDate = message.ProcessDate.ToString("dd/MM/yyyy HH:mm:ss"),
				LastUpdateDate = message.LastUpdateDate.ToString("dd/MM/yyyy HH:mm:ss")
			};
		}
	}
}
