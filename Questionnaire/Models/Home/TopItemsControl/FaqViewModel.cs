using Questionnaire.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Models {
	public class FaqViewModel {
		public int Id { get; set; }
		public int RecordId { get; set; }
		public string Name { get; set; }

		public static implicit operator FaqViewModel(Faq faq) {
			return new FaqViewModel {
				Id = faq.Id,
				RecordId = faq.RecordId,
				Name = faq.Name,
			};
		}
	}
}
