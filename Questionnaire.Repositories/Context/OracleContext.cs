using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Repositories {
	public class OracleContext : IDisposable {
		private string _connectionString;

		public OracleContext() {
			_connectionString = ConfigurationManager.ConnectionStrings["OracleContext"].ConnectionString;
		}

		public OracleConnection Connection {
			get { return _connection ?? (_connection = new OracleConnection(_connectionString)); }
		}
		private OracleConnection _connection;

		public DataTable GetTable(string cmdText, params OracleParameter[] parametres) {
			var cmd = GetCommand(cmdText, parametres);
			try {
				OpenConnection(cmd);
				DataTable dt = new DataTable();
				new OracleDataAdapter(cmd).Fill(dt);
				return dt;
			} catch(Exception ex) {
				string message = ex.Message;
				return null;
			} finally { CloseConnection(); }
		}

		public OracleCommand ExecuteCommand(string cmdText, List<OracleParameter> parametres) {
			var cmd = GetCommand(cmdText, parametres.ToArray());
			try {
				OpenConnection(cmd);
				var res = cmd.ExecuteNonQuery();
				return cmd;
			} catch(Exception ex) {
				return null;
			}
		}

		public OracleDataReader ExecuteReader(string cmdText, params OracleParameter[] parametres) {
			var cmd = GetCommand(cmdText, parametres);
			try {
				OpenConnection(cmd);
				return cmd.ExecuteReader();
			} catch(Exception ex) {
				return null;
			} finally { cmd.Connection.Close(); }
		}

		public object ExecuteScalar(string cmdText, params OracleParameter[] parametres) {
			var cmd = GetCommand(cmdText, parametres);
			try {
				OpenConnection(cmd);
				return cmd.ExecuteScalar();
			} catch(Exception ex) {
				return null;
			} finally { cmd.Connection.Close(); }
		}

		public void ExecuteNonQuery(string cmdText, params OracleParameter[] parametres) {
			var cmd = GetCommand(cmdText, parametres);
			try {
				OpenConnection(cmd);
				cmd.ExecuteNonQuery();
			} catch(Exception ex) {
			} finally { cmd.Connection.Close(); }
		}

		public string CreateCommandText(string cmd) {
			return "begin :data := " + cmd + " end;";
		}

		private OracleCommand GetCommand(string cmdText, params OracleParameter[] parametres) {
			return GetCommand(cmdText, CommandType.Text, parametres);
		}

		private OracleCommand GetCommand(string cmdText, CommandType commandType, params OracleParameter[] parametres) {
			OracleCommand cmd = new OracleCommand(cmdText, this.Connection);
			cmd.CommandType = commandType;
			foreach(var p in parametres) cmd.Parameters.Add(p);
			return cmd;
		}

		private void OpenConnection(OracleCommand cmd) {
			if(cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
		}

		private void CloseConnection() {
			try { _connection.Close(); } catch { }
		}

		public void Dispose() {
			//throw new NotImplementedException();
		}
	}
}