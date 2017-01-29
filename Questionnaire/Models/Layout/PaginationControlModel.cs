using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Models
{
    public class PaginationControlModel
    {
        public int PageNo { get; set; }
        public int TotalPages { get; set; }
        public int ResultPerPage { get; set; }
    }
}
