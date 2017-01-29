using Questionnaire.Domain.Entities;
using System;
using System.Collections.Generic;
using Questionnaire.Infrastructure.Utils;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Models {
	public class GetEmailAnswerListView {
		public int TreeId { get; set; }
		public List<EmailAnswerComparisonItem> Answers { get; set; }
	}

	public class EmailAnswerComparisonItem {
		public int Id { get; set; }
		public DateTime FromDate { get; set; }
		public DateTime ToDate { get; set; }
		public double? FromDateEpoch { get; set; }
		public string Filename { get; set; }
		public string Comment { get; set; }
		public string Username { get; set; }

		public string Name {
			get {
				return this.FromDate.ToString("dd.MM.yyyy HH:mm:ss");
			}
		}
		public bool Checked { get; set; }

		public static implicit operator EmailAnswerComparisonItem(AnswerVersion answerVersion) {
			return new EmailAnswerComparisonItem {
				Id = answerVersion.Id,
				FromDate = answerVersion.FromDate,
				ToDate = answerVersion.ToDate,
				Filename = answerVersion.FileName,
				Username = answerVersion.Username,
				Comment = answerVersion.Comment,
				FromDateEpoch = answerVersion.FromDate.ToEpoch()
			};
		}
	}
}
