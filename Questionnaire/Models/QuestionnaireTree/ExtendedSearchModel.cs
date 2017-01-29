using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Models {
	public class ExtendedSearchModel {
		public int SearchType { get; set; }
		public string SearchTerm { get; set; }
		public string SearchComment { get; set; }
		public DateTime Date { get; set; }
		public double? PeriodStartEpoch { get; set; }
		public double? PeriodEndEpoch { get; set; }
		public int StatusId { get; set; }
		public int OwnerId { get; set; }
		public int OnlyPublished { get; set; }
		public List<DropDownViewModel> Statuses { get; set; }
		public List<DropDownViewModel> Owners { get; set; }
		public List<DropDownViewModel> OnlyPublishedList { get; set; }
		public bool FromToIsActive { get; set; }
		public bool StatusIsActive { get; set; }
		public bool OwnerIsActive { get; set; }
		public bool IsHistoryOn { get; set; }
		public double HistoryDate { get; set; }
	}
}
