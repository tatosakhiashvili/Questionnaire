using Questionnaire.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Domain.Entities;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace Questionnaire.Repositories {
	public class InternationalCodeRepository : IInternationalCodeRepository {

		private OracleContext _context;

		public InternationalCodeRepository(OracleContext context) {
			_context = context;
		}

		public IEnumerable<Code> GetInternationalCodes(decimal userId, int languageId) {
			var cmd = _context.CreateCommandText("pg_faq_select.get_int_codes(:p_user_id, :p_record_count);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.RefCursor, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_record_count", OracleDbType.Decimal, null, ParameterDirection.Output));

			var result = _context.GetTable(cmd, oracleParams.ToArray());

			var codes = result.AsEnumerable().Select(x => new Code {
				Cc = x["cc"] == DBNull.Value ? "" : (string)x["cc"],
				NameGeo = x["name_geo"] == DBNull.Value ? "" : (string)x["name_geo"],
				NameEng = x["name_eng"] == DBNull.Value ? "" : (string)x["name_eng"],
				TelCode = x["tel_code"] == DBNull.Value ? "" : (string)x["tel_code"],
				CityNameGeo = x["city_name_geo"] == DBNull.Value ? "" : (string)x["city_name_geo"],
				CityNameEng = x["city_name_eng"] == DBNull.Value ? "" : (string)x["city_name_eng"],
				SmsText = x["sms_text"] == DBNull.Value ? "" : (string)x["sms_text"],
				SmsTextCity = x["sms_text_city"] == DBNull.Value ? "" : (string)x["sms_text_city"],
			});

			return codes;
		}
	}
}
