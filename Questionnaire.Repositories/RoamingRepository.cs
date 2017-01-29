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
	public class RoamingRepository : IRoamingRepository {

		private OracleContext _context;

		public RoamingRepository(OracleContext context) {
			_context = context;
		}

		public IEnumerable<Country> GetCountries(decimal userId, int languageId) {
			var cmd = _context.CreateCommandText("pg_faq_select.get_roaming_countries(:p_user_id, :p_record_count);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.RefCursor, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_record_count", OracleDbType.Decimal, null, ParameterDirection.Output));

			var result = _context.GetTable(cmd, oracleParams.ToArray());

			var countries = result.AsEnumerable().Select(x => new Country {
				Id = Convert.ToInt32(x["t1201_id"]),
				NameGeo = x["country_geo"] == DBNull.Value ? "" : (string)x["country_geo"],
				NameEng = x["country_eng"] == DBNull.Value ? "" : (string)x["country_eng"],
				Code = x["country_code"] == DBNull.Value ? "" : (string)x["country_code"]
			});

			return countries;
		}

		public IEnumerable<Operator> GetOperators(int countryId, decimal userId, int languageId) {
			var cmd = _context.CreateCommandText("pg_faq_select.get_roaming_operators(:p_country_id, :p_user_id, :p_record_count);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.RefCursor, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_country_id", OracleDbType.Decimal, countryId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_record_count", OracleDbType.Decimal, null, ParameterDirection.Output));

			var result = _context.GetTable(cmd, oracleParams.ToArray());

			var operators = result.AsEnumerable().Select(x => new Operator {
				Id = Convert.ToInt32(x["t1202_id"]),
				Name = x["operator_name"] == DBNull.Value ? "" : (string)x["operator_name"],
				NetworkCode = x["network_code"] == DBNull.Value ? "" : (string)x["network_code"],
				NetworkType = x["network_type"] == DBNull.Value ? "" : (string)x["network_type"],
				Display = x["display"] == DBNull.Value ? "" : (string)x["display"],
				CustomerCare = x["customer_care"] == DBNull.Value ? "" : (string)x["customer_care"],
				GprsRoamingStatus = x["gprs_roaming_status"] == DBNull.Value ? "" : (string)x["gprs_roaming_status"]
			});

			return operators;
		}

		public IEnumerable<Tariff> GetTariffs(int operatorId, decimal userId, int languageId) {
			var cmd = _context.CreateCommandText("pg_faq_select.get_roaming_tariffs(:p_operator_id, :p_user_id, :p_record_count);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.RefCursor, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_operator_id", OracleDbType.Decimal, operatorId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_record_count", OracleDbType.Decimal, null, ParameterDirection.Output));

			var result = _context.GetTable(cmd, oracleParams.ToArray());
			var tariffs = new List<Tariff> { };

			var columns = new List<string> { };
			foreach(var column in result.Columns) { columns.Add(column.ToString()); }

			var keyValuePair = new Dictionary<string, object>();
			var resultList = result.AsEnumerable().ToList();
			var firstItem = resultList.FirstOrDefault();

			foreach(var column in columns) {
				keyValuePair.Add(column, firstItem[column]);
			}

			tariffs.Add(new Tariff {
				EmailSubject = firstItem["email_subject"].ToString(),
				TariffParameters = keyValuePair
			});

			return tariffs;
		}
	}
}