using Questionnaire.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Domain;

namespace Questionnaire.Repositories.DummyRepositories {
	public class StoreRepository : IStoreRepository {
		public StoreRepository() {

		}

		public IEnumerable<Product> GetProducts(decimal userId, int languageId) {
			return new List<Product> {
				 new Product { Id = 1, Name = "Product 1" },
				 new Product { Id = 2, Name = "Product 2" },
			};
		}

		public IEnumerable<ProductDetail> GetProductDetails(int productId, decimal userId, int languageId) {
			return new List<ProductDetail> {
				new ProductDetail { Name = "Name One", Cnt = 1 },
				new ProductDetail { Name = "Name Oni", Cnt = 2 },
				new ProductDetail { Name = "Name Three", Cnt = 3 }
			};
		}
	}
}
