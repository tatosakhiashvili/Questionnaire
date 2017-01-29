using Questionnaire.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Domain;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Questionnaire.Repositories {
	public class StoreRepository : IStoreRepository {
		private OracleContext _context;

		public StoreRepository(OracleContext context) {
			_context = context;
		}

		public IEnumerable<Product> GetProducts(decimal userId, int languageId) {
			var cmd = _context.CreateCommandText("pg_faq_select.get_gs_products_list(:p_user_id, :p_record_count);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.RefCursor, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_record_count", OracleDbType.Decimal, null, ParameterDirection.Output));

			var result = _context.GetTable(cmd, oracleParams.ToArray());

			var products = result.AsEnumerable().Select(x => new Product {
				Id = Convert.ToInt32(x["t2102_id"]),
				Name = x["product_name"] == DBNull.Value ? "" : (string)x["product_name"],
			});

			return products;
		}

		public IEnumerable<ProductDetail> GetProductDetails(int productId, decimal userId, int languageId) {
			var cmd = _context.CreateCommandText("pg_faq_select.get_gs_prod_balance(:p_prod_id, :p_user_id, :p_record_count);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.RefCursor, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_prod_id", OracleDbType.Decimal, productId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_record_count", OracleDbType.Decimal, null, ParameterDirection.Output));

			var result = _context.GetTable(cmd, oracleParams.ToArray());

			var productsDetails = result.AsEnumerable().Select(x => new ProductDetail {
				Name = x["depot_name"] == DBNull.Value ? "" : (string)x["depot_name"],
				Cnt = Convert.ToDecimal(x["cnt"]),
			});

			return productsDetails;
		}
	}
}