using Questionnaire.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Domain.Entities;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using Oracle.ManagedDataAccess.Types;

namespace Questionnaire.Repositories {
	public class UserRepository : IUserRepository {
		private ICacheRepository _cacheRepository;
		private OracleContext _context;

		public UserRepository(ICacheRepository cacheRepository, OracleContext context) {
			_cacheRepository = cacheRepository;
			_context = context;
		}

		public User Fetch(string username, string password) {
			var cmd = _context.CreateCommandText("pg_users_ws.login_ws(:p_user_name, :p_pass, :p_other_user_name, :p_user_id, :p_fname, :p_lname, :p_group_id, :p_group_name, :verrdsc, :voraerror);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.Decimal, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_user_name", OracleDbType.Varchar2, username, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_pass", OracleDbType.Varchar2, password, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_other_user_name", OracleDbType.Varchar2, null, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, 0, ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("p_fname", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("p_lname", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("p_group_id", OracleDbType.Decimal, 0, ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("p_group_name", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("verrdsc", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("voraerror", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));

			var result = _context.ExecuteCommand(cmd, oracleParams);

			if(result == null) { return null; }
			var user = new User {
				Id = (decimal)(OracleDecimal)result.Parameters["p_user_id"].Value,
				Username = username,
				FirstName = (string)(OracleString)result.Parameters["p_fname"].Value,
				LastName = (string)(OracleString)result.Parameters["p_lname"].Value,
				GroupId = (decimal)(OracleDecimal)result.Parameters["p_group_id"].Value,
				GroupName = (string)(OracleString)result.Parameters["p_group_name"].Value,
				Roles = new List<Role> { }
			};

			if(user.GroupId == 122) { // 122 is admin's group id
				user.Roles.Add(new Role { Id = 1, Name = "Admin" });
			}

			return user;
		}
	}
}