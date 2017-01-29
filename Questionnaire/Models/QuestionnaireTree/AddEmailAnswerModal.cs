using Questionnaire.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Models {
	public class AddEmailAnswerModal {
		public int? NodeId { get; set; }
		public double? FromDateEpoch { get; set; }
		public double? ToDateEpoch { get; set; }
		public int? AnswerId { get; set; }
		public int HourId { get; set; }
		public string Answer { get; set; }
		public string PureAnswer { get; set; }
		public string Comment { get; set; }
		public bool SaveAsTemplate { get; set; }
		public string TemplateName { get; set; }
		public List<EmailTemplate> Templates { get; set; }
		public List<EmailAnswerFile> Files { get; set; }

		public List<DropDownViewModel> Hours { get; set; }
		public List<DropDownCheckboxViewModel> EmailList { get; set; }
		//public List<DropDownViewModel> Statuses { get; set; }
	}

	public class EmailTemplate {
		public int Id { get; set; }
		public string Name { get; set; }
		public string FileName { get; set; }

		public static implicit operator EmailTemplate(AnswerTemplate template) {
			return new EmailTemplate {
				Id = template.Id,
				Name = template.Name,
				FileName = template.FileName
			};
		}
	}

	public class EmailAnswerFile {
		public int Id { get; set; }
		public int AnswerId { get; set; }
		public string Name { get; set; }
		public string FolderName { get; set; }
		public string Comment { get; set; }
		public bool IsTemporary { get; set; }
	}
}
