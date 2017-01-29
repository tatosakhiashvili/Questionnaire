using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Models {
	public class MaintainTreeItemViewMode {
		public int TreeId { get; set; }
		public int SelectedNodeId { get; set; }
		public int? TreeToId { get; set; }
		public bool LockTree { get; set; }
		public bool AllowCopyChildrens { get; set; }
		public bool CopyChildrens { get; set; }
		public double? FromDateEpoch { get; set; }
		public double? ToDateEpoch { get; set; }
		public double? CreateDateEpoch { get; set; }
		public int? NewsDays { get; set; }
		public int? Status { get; set; }
		public List<DropDownViewModel> Statuses { get; set; }
		public string Text { get; set; }
		public string Comment { get; set; }
		public string PreviousChangeMadeEditor { get; set; }
		public ModifyStatus ModifyStatus { get; set; }
		public QuestionnaireViewModel TreeModel { get; set; }
		public List<TreeFileViewModel> Files { get; set; }
	}

	public class TreeFileViewModel {
		public int Id { get; set; }
		public string FileName { get; set; }
		public string FolderName { get; set; }
		public bool IsTemporary { get; set; }
		public string Comment { get; set; }
	}

	public enum YesOrNoStatusEnum {
		No = 0,
		Yes = 1
	}

	public enum TreeStatusEnum {
		Published = 1,
		Prepublished = 2,
		Deleted = 3
	}

	public enum ModifyStatus {
		AddTreeNode = 1,
		ChangeTreeNode = 2,
		CopyTreeNode = 3,
		MoveTreeNode = 4
	}
}
