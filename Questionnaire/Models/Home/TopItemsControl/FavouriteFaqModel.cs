using Questionnaire.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Models
{
    public class FavouriteFaqModel : FaqBaseModel
    {
        public static implicit operator FavouriteFaqModel(List<Faq> faqs)
        {
            return new FavouriteFaqModel
            {
                Faqs = faqs.Select(x => (FaqViewModel)x).ToList()
            };
        }
    }
}