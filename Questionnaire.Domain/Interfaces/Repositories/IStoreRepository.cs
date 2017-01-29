using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Domain.Interfaces.Repositories {
	public interface IStoreRepository {
		IEnumerable<Product> GetProducts(decimal userId, int languageId);
		IEnumerable<ProductDetail> GetProductDetails(int productId, decimal userId, int languageId);
	}
}
