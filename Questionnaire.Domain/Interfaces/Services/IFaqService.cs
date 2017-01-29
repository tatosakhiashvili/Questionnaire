using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Domain.Interfaces.Services {
	public interface IFaqService {
		IEnumerable<Faq> GetFavouriteFaq();
		IEnumerable<Faq> GetTopFaq(TopFaqEnumeration topType);
		IEnumerable<Faq> GetCustomerFaq(string msisdn);
	}
}
