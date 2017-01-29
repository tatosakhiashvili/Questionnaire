using Questionnaire.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Models {
	public class ProductViewModel {
		public List<ProductItemViewModel> Products { get; set; }
	}

	public class ProductItemViewModel {
		public int Id { get; set; }
		public string Name { get; set; }

		public static implicit operator ProductItemViewModel(Product product) {
			return new ProductItemViewModel {
				Id = product.Id,
				Name = product.Name
			};
		}
	}
}
