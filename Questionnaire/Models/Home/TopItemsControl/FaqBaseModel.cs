using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Models
{
    public class FaqBaseModel
    {
        public List<FaqViewModel> Faqs { get; set; }
        public int CurrentPage { get; set; }
        public int TotalCount { get { return Faqs.Count; } }
        public int PageCount { get { return TotalCount % ResultsPerPage == 0 ? TotalCount / ResultsPerPage : ((TotalCount / ResultsPerPage) + 1); } }
        public int ResultsPerPage { get { return 5; } } //TODO: From config
    }
}