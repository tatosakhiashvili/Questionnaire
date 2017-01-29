using Questionnaire.Domain.Entities;
using Questionnaire.Domain.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Models {
	public class ChatItemDetailsViewModel {
		public int MessageId { get; set; }
		public List<ChatItemStatusViewModel> Statuses { get; set; }
	}

	public class ChatItemStatusViewModel {
		public int Id { get; set; }
		public string Name { get; set; }
		public string Date { get; set; }
		public string Operator { get; set; }

		public static implicit operator ChatItemStatusViewModel(InternalMessageStatus status) {
			return new ChatItemStatusViewModel {
				Id = status.Id,
				Name = status.Name,
				Operator = status.Operator,
				Date = status.CreateDate.ToString("dd/MM/yyyy HH:mm:ss")
			};
		}
	}

	public class ChatItemFileViewModel {
		public int Id { get; set; }
		public string FolderName { get; set; }
		public bool IsTemporary { get; set; }
		public string FileName { get; set; }
		public string Operator { get; set; }

		public static implicit operator ChatItemFileViewModel(InternalMessageFile file) {
			var folderDestinationPath = UtilExtenders.MapPath("~/Uploads/ChatFiles/" + file.MessageId + "/" + file.Name);
			var fileName = new DirectoryInfo(folderDestinationPath).GetFiles().FirstOrDefault()?.Name;

			return new ChatItemFileViewModel {
				Id = file.Id,
				Operator = file.Operator,
				FolderName = file.Name,
				FileName = fileName
			};
		}
	}
}
