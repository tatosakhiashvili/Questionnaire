using Questionnaire.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Models
{
    public class CustomerFaqModel : FaqBaseModel
    {
        public static implicit operator CustomerFaqModel(List<Faq> faqs)
        {
            return new CustomerFaqModel
            {
                Faqs = faqs.Select(x => (FaqViewModel)x).ToList()
            };
        }
    }
}