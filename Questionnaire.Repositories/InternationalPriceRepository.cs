using Questionnaire.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Domain.Entities;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using Questionnaire.Domain;

namespace Questionnaire.Repositories {
	public class InternationalPriceRepository : IInternationalPriceRepository {

		private OracleContext _context;

		public InternationalPriceRepository(OracleContext context) {
			_context = context;
		}

		public IEnumerable<Company> GetCompanies(decimal userId, int languageId) {
			var cmd = _context.CreateCommandText("pg_faq_select.get_int_companies(:p_user_id, :p_record_count);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.RefCursor, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_record_count", OracleDbType.Decimal, null, ParameterDirection.Output));

			var result = _context.GetTable(cmd, oracleParams.ToArray());

			var prices = result.AsEnumerable().Select(x => new Company {
				Name = x["company_name"] == DBNull.Value ? "" : (string)x["company_name"],
				Code = x["company_code"] == DBNull.Value ? "" : (string)x["company_code"],
			});

			return prices;
		}

		public IEnumerable<Country> GetCountries(decimal userId, int languageId) {
			var cmd = _context.CreateCommandText("pg_faq_select.get_countries_list(:p_user_id, :p_record_count);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.RefCursor, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_record_count", OracleDbType.Decimal, null, ParameterDirection.Output));

			var result = _context.GetTable(cmd, oracleParams.ToArray());

			var countries = result.AsEnumerable().Select(x => new Country {
				Id = 0,
				Code = x["cc"] == DBNull.Value ? "" : (string)x["cc"],
				NameGeo = x["name_geo"] == DBNull.Value ? "" : (string)x["name_geo"],
				NameEng = x["name_eng"] == DBNull.Value ? "" : (string)x["name_eng"]
			});

			return countries;
		}

		public IEnumerable<InternationalTariff> GetInternationalTariffrs(string countryCode, string companyCode, decimal userId, int languageId) {
			var cmd = _context.CreateCommandText("pg_faq_select.get_int_tariffs(:p_ind, :p_code, :p_user_id, :p_record_count);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.RefCursor, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_ind", OracleDbType.Decimal, companyCode, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_code", OracleDbType.Decimal, countryCode, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_record_count", OracleDbType.Decimal, null, ParameterDirection.Output));

			var result = _context.GetTable(cmd, oracleParams.ToArray());

			var reminderItems = result.AsEnumerable().Select(x => new InternationalTariff {
				ZonIndName = x["zon_ind_name"] == DBNull.Value ? "" : (string)x["zon_ind_name"],
				Ind = x["ind"] == DBNull.Value ? "" : (string)x["ind"],
				PriceC = x["price_c"] == DBNull.Value ? "" : (string)x["price_c"],
				PriceRound = Convert.ToDecimal(x["price_round"]),
				SmsText = x["sms_text"] == DBNull.Value ? "" : (string)x["sms_text"],
				Units = Convert.ToDecimal(x["units"]),
			});

			return reminderItems;
		}
	}
}
