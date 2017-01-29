using Questionnaire.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Models
{
    public class TopFaqModel : FaqBaseModel
    {
        public TopFaqEnumeration TopType { get; set; }

        public static implicit operator TopFaqModel(List<Faq> faqs)
        {
            return new TopFaqModel
            {
                Faqs = faqs.Select(x => (FaqViewModel)x).ToList()
            };
        }
    }
}