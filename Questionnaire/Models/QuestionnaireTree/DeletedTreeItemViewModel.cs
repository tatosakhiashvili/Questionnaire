using Questionnaire.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Questionnaire.Models {

	public class DeletedTreeItemsViewModel {
		public List<DeletedTreeItemViewModel> Records { get; set; }
	}

	public class DeletedTreeItemViewModel {
		public int Id { get; set; }
		public string Caption { get; set; }
		public string FromDate { get; set; }
		public string ToDate { get; set; }
		public string Username { get; set; }
		public string Comments { get; set; }
		public string IsNew { get; set; }
		public string DeleteType { get; set; }

		public static implicit operator DeletedTreeItemViewModel(DeletedTree deletedTree) {
			return new DeletedTreeItemViewModel {
				Id = deletedTree.Id,
				Caption = deletedTree.Caption,
				FromDate = deletedTree.FromDate.ToString("dd/MM/yyyy hh:mm:ss"),
				ToDate = deletedTree.ToDate.ToString("dd/MM/yyyy hh:mm:ss"),
				Username = deletedTree.Username,
				Comments = deletedTree.Comments,
				IsNew = deletedTree.IsNew ? "Yes" : "No",
				DeleteType = deletedTree.DeleteType
			};
		}
	}
}