using Questionnaire.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Questionnaire.Models {
	public class ProductDetailsViewModel {
		public List<ProductDetailItemViewModel> ProductDetails { get; set; }
	}

	public class ProductDetailItemViewModel {
		public string Name { get; set; }
		public string Cnt { get; set; }

		public static implicit operator ProductDetailItemViewModel(ProductDetail productDetail) {
			return new ProductDetailItemViewModel {
				Name = productDetail.Name,
				Cnt = productDetail.Cnt.ToString()
			};
		}
	}
}