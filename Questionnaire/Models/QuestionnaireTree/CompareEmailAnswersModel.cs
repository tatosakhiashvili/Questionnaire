using Questionnaire.Domain.Entities;
using Questionnaire.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Models {
	public class CompareEmailAnswersModel {

		public int TreeId { get; set; }

		public string OldTemplateCaption { get; set; }
		public string NewTemplateCaption { get; set; }

		public string OldTemplateUsername { get; set; }
		public string NewTemplateUsername { get; set; }

		public string OldTemplateComment { get; set; }
		public string NewTemplateComment { get; set; }

		public List<CompareEmailFileViewModel> OldFiles { get; set; }
		public List<CompareEmailFileViewModel> NewFiles { get; set; }

		public DifferenceFinderModel Difference { get; set; }
	}

	public class CompareEmailFileViewModel {
		public int TreeId { get; set; }
		public string FileName { get; set; }
		public string FolderName { get; set; }

		public static implicit operator CompareEmailFileViewModel(EmailComparisonFile file) {
			var fileName = "";
			var directoryPath = UtilExtenders.MapPath("~/Uploads/EmailAnswerFiles/" + file.TreeId + "/" + file.FolderName);
			if(System.IO.Directory.Exists(directoryPath)) {
				var files = System.IO.Directory.GetFiles(directoryPath);
				if(files != null && files.Count() > 0) {
					var fileInfo = new System.IO.FileInfo(files.FirstOrDefault());
					fileName = fileInfo.Name;
				}
			}

			return new CompareEmailFileViewModel {
				FolderName = file.FolderName,
				TreeId = file.TreeId,
				FileName = fileName
			};
		}
	}
}
