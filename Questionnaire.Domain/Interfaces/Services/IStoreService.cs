using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Domain.Interfaces.Services {
	public interface IStoreService {
		IEnumerable<Product> GetProducts();
		IEnumerable<ProductDetail> GetProductDetails(int productId);
	}
}