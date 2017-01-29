using Questionnaire.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Domain;
using Questionnaire.Domain.Interfaces.Repositories;

namespace Questionnaire.Services {
	public class StoreService : IStoreService {

		private IStoreRepository _storeRepository;

		public StoreService(IStoreRepository storeRepository) {
			_storeRepository = storeRepository;
		}

		public IEnumerable<Product> GetProducts() {
			return _storeRepository.GetProducts(QuestionnaireContext.Current.UserId, QuestionnaireContext.Current.LanguageId);
		}

		public IEnumerable<ProductDetail> GetProductDetails(int productId) {
			return _storeRepository.GetProductDetails(productId, QuestionnaireContext.Current.UserId, QuestionnaireContext.Current.LanguageId);
		}
	}
}
