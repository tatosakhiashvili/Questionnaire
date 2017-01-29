using Questionnaire.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Domain.Entities;
using Questionnaire.Domain;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Questionnaire.Repositories {
	public class ChatRepository : IChatRepository {
		private OracleContext _context;

		public ChatRepository(OracleContext context) {
			_context = context;
		}

		public int AddInternalMessage(string text, int typeId, int priorityId, string comment, DateTime fromDate, DateTime toDate, DateTime processDate, decimal userId, int languageId) {
			var cmd = _context.CreateCommandText("pg_faq_ws.add_im_list_ws(:p_id, :p_fd, :p_td, :p_message_text, :p_type_id, :p_process_date, :p_priority_id, :p_user_id, :p_comments, :vpg, :vfn, :verrdsc, :voraerror);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.Decimal, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_id", OracleDbType.Decimal, -1, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_fd", OracleDbType.Date, null, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_td", OracleDbType.Date, null, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_message_text", OracleDbType.NVarchar2, text, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_type_id", OracleDbType.Decimal, typeId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_process_date", OracleDbType.Date, processDate, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_priority_id", OracleDbType.Decimal, priorityId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_comments", OracleDbType.NVarchar2, comment, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("vpg", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("vfn", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("verrdsc", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("voraerror", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));

			var result = _context.ExecuteCommand(cmd, oracleParams);
			var messageId = result.Parameters["data"].Value;

			return Convert.ToInt32(messageId?.ToString() ?? "0");			
		}

		public bool ChangeInternalMessage(int id, string text, int typeId, int priorityId, string comment, DateTime fromDate, DateTime toDate, DateTime processDate, decimal userId, int languageId) {
			var cmd = _context.CreateCommandText("pg_faq_ws.add_im_list_ws(:p_id, :p_fd, :p_td, :p_message_text, :p_type_id, :p_process_date, :p_priority_id, :p_user_id, :p_comments, :vpg, :vfn, :verrdsc, :voraerror);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.Decimal, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_id", OracleDbType.Decimal, id, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_fd", OracleDbType.Date, null, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_td", OracleDbType.Date, null, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_message_text", OracleDbType.NVarchar2, text, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_type_id", OracleDbType.Decimal, typeId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_process_date", OracleDbType.Date, processDate, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_priority_id", OracleDbType.Decimal, priorityId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_comments", OracleDbType.NVarchar2, comment, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("vpg", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("vfn", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("verrdsc", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("voraerror", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));

			var result = _context.ExecuteCommand(cmd, oracleParams);
			return result != null;
		}

		public bool DeleteInternalMessage(int id, string comment, decimal userId, int languageId) {
			var cmd = _context.CreateCommandText("pg_faq_ws.del_im_ws(:p_fd, :p_im_id, :p_user_id, :p_comments, :vpg, :vfn, :verrdsc, :voraerror);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.Decimal, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_fd", OracleDbType.Date, null, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_im_id", OracleDbType.Decimal, id, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_comments", OracleDbType.NVarchar2, comment, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("vpg", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("vfn", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("verrdsc", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("voraerror", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));

			var result = _context.ExecuteCommand(cmd, oracleParams);
			return result != null;
		}

		public bool ExecuteInternalMessage(int id, string comment, decimal userId, int languageId) {
			var cmd = _context.CreateCommandText("pg_faq_ws.process_im_ws(:p_fd, :p_im_id, :p_user_id, :p_comments, :vpg, :vfn, :verrdsc, :voraerror);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.Decimal, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_fd", OracleDbType.Date, null, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_im_id", OracleDbType.Decimal, id, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_comments", OracleDbType.NVarchar2, comment, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("vpg", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("vfn", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("verrdsc", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("voraerror", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));

			var result = _context.ExecuteCommand(cmd, oracleParams);
			return result != null;
		}

		public InternalMessage GetInternalMessage(int id, decimal userId, int languageId) {
			var messages = GetInternalMessages(userId, languageId); //TODO: This should come from cache
			return messages.FirstOrDefault(x => x.Id == id);
		}

		public IEnumerable<InternalMessageFile> GetInternalMessageFiles(int id, decimal userId, int languageId) {
			var cmd = _context.CreateCommandText("pg_faq_select.get_im_attachments(:p_im_id, :p_user_id, :p_record_count);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.RefCursor, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_im_id", OracleDbType.Decimal, id, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_record_count", OracleDbType.Decimal, null, ParameterDirection.Output));

			var result = _context.GetTable(cmd, oracleParams.ToArray());

			var messagePrioritiesResult = result.AsEnumerable().Select(x => new InternalMessageFile {
				Id = Convert.ToInt32(x["t1116_id"]),
				Name = Convert.ToString(x["file_name"]),
				Operator = Convert.ToString(x["operator_name"]),
				CreateDate = Convert.ToDateTime(x["fd"]),
				MessageId = id
			}).ToList();

			return messagePrioritiesResult;
		}

		public IEnumerable<InternalMessagePriority> GetInternalMessagePriorities(decimal userId, int languageId) {
			var cmd = _context.CreateCommandText("pg_faq_select.get_im_priority_list(:p_user_id, :p_record_count);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.RefCursor, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_record_count", OracleDbType.Decimal, null, ParameterDirection.Output));

			var result = _context.GetTable(cmd, oracleParams.ToArray());

			var messagePrioritiesResult = result.AsEnumerable().Select(x => new InternalMessagePriority {
				Id = Convert.ToInt32(x["id"]),
				Name = (string)x["priority"],
			}).ToList();

			return messagePrioritiesResult;
		}

		public IEnumerable<InternalMessage> GetInternalMessages(decimal userId, int languageId) {
			var cmd = _context.CreateCommandText("pg_faq_select.get_im_list(:p_status_id, :p_user_id, :p_record_count);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.RefCursor, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_status_id", OracleDbType.Decimal, 0, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_record_count", OracleDbType.Decimal, null, ParameterDirection.Output));

			var result = _context.GetTable(cmd, oracleParams.ToArray());

			var messagesResult = result.AsEnumerable().Select(x => new InternalMessage {
				Id = Convert.ToInt32(x["t1115_id"]),
				ProcessDate = Convert.ToDateTime(x["process_date"]),
				TypeId = Convert.ToInt32(x["type_id"]),
				Priority = Convert.ToString(x["priority_name"]),
				Comment = Convert.ToString(x["comments"]),
				CreateDate = Convert.ToDateTime(x["create_date"]),
				LastUpdateDate = DateTime.Now,
				Operator = Convert.ToString(x["operator_name"]),
				Owner = Convert.ToString(x["owner_name"]),
				PriorityId = Convert.ToInt32(x["priority_id"]),
				Status = Convert.ToString(x["status_name"]),
				StatusId = Convert.ToInt32(x["status_id"]),
				Text = Convert.ToString(x["message_text"])
			}).ToList();

			return messagesResult;
		}

		public IEnumerable<InternalMessageStatus> GetInternalMessageStatuses(int id, decimal userId, int languageId) {
			var cmd = _context.CreateCommandText("pg_faq_select.get_im_status(:p_im_id, :p_user_id, :p_record_count);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.RefCursor, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_im_id", OracleDbType.Decimal, id, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_record_count", OracleDbType.Decimal, null, ParameterDirection.Output));

			var result = _context.GetTable(cmd, oracleParams.ToArray());

			var messageStatusesResult = result.AsEnumerable().Select(x => new InternalMessageStatus {
				Id = Convert.ToInt32(x["t1117_id"]),
				Name = (string)x["status_name"],
				Operator = (string)x["operator_name"],
				CreateDate = Convert.ToDateTime(x["in_date"])
			}).ToList();

			return messageStatusesResult;
		}

		public IEnumerable<InternalMessageType> GetInternalMessageTypes(decimal userId, int languageId) {
			var cmd = _context.CreateCommandText("pg_faq_select.get_im_types_list(:p_user_id, :p_record_count);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.RefCursor, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_record_count", OracleDbType.Decimal, null, ParameterDirection.Output));

			var result = _context.GetTable(cmd, oracleParams.ToArray());
			var messageTypesResult = result.AsEnumerable().Select(x => new InternalMessageType {
				Id = Convert.ToInt32(x["id"]),
				Name = (string)x["type_name"],
			}).ToList();

			return messageTypesResult;
		}

		public void RemoveFile(int messageId, string fileName, decimal userId, int languageId) {
			var cmd = _context.CreateCommandText("pg_faq_ws.del_im_attachment(:p_id, :p_fd, :p_user_id, :p_comments, :vpg, :vfn, :verrdsc, :voraerror);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.Decimal, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_id", OracleDbType.Decimal, messageId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_fd", OracleDbType.Date, null, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_comments", OracleDbType.NVarchar2, string.Empty, ParameterDirection.Input));

			oracleParams.Add(new OracleParameter("vpg", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("vfn", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("verrdsc", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("voraerror", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));

			var result = _context.ExecuteCommand(cmd, oracleParams);
		}

		public void SaveFile(int messageId, string fileName, decimal userId, int languageId) {
			var cmd = _context.CreateCommandText("pg_faq_ws.mod_im_attachments_ws(:p_id, :p_fd, :p_td, :p_im_id, :p_link, :p_blob_name, :p_blob_dsc, :p_blob_caption, :p_blob_date, :p_blob_type, :p_blob, :p_user_id, :p_comments, :vpg, :vfn, :verrdsc, :voraerror);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.Decimal, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_id", OracleDbType.Decimal, -1, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_fd", OracleDbType.Date, null, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_td", OracleDbType.Date, null, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_im_id", OracleDbType.Decimal, messageId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_link", OracleDbType.Varchar2, fileName, ParameterDirection.Input));

			oracleParams.Add(new OracleParameter("p_blob_name", OracleDbType.Varchar2, fileName, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_blob_dsc", OracleDbType.Varchar2, "", ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_blob_caption", OracleDbType.Varchar2, "", ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_blob_date", OracleDbType.Date, DateTime.Now, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_blob_type", OracleDbType.NVarchar2, string.Empty, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_blob", OracleDbType.Blob, null, ParameterDirection.Input));

			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_comments", OracleDbType.NVarchar2, "", ParameterDirection.Input));

			oracleParams.Add(new OracleParameter("vpg", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("vfn", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("verrdsc", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("voraerror", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));


			_context.ExecuteCommand(cmd, oracleParams);
		}
	}
}
