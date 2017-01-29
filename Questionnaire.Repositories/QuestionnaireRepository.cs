using Questionnaire.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Domain;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using Newtonsoft.Json;
using Questionnaire.Domain.Entities;

namespace Questionnaire.Repositories {
	public class QuestionnaireRepository : IQuestionnaireRepository {
		private OracleContext _context;
		private ICacheRepository _cacheRepository;

		public QuestionnaireRepository(OracleContext context, ICacheRepository cacheRepository) {
			_context = context;
			_cacheRepository = cacheRepository;
		}

		public List<Tree> GetFaqTree(DateTime date, bool onlyPublished, int languageId, decimal userId) {
			var cmd = _context.CreateCommandText("pg_faq_select.get_faq_tree(:p_date,:p_only_published,:p_lang_id,:p_user_id,:p_record_count);");
			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.RefCursor, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_date", OracleDbType.Date, date, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_only_published", OracleDbType.Char, onlyPublished, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_lang_id", OracleDbType.Decimal, languageId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_record_count", OracleDbType.Decimal, null, ParameterDirection.Input));

			var table = _context.GetTable(cmd, oracleParams.ToArray());

			var treeItems = table.AsEnumerable().Select(x => new Tree {
				Id = Convert.ToInt32(x["t1101_id"]),
				Text = (string)x["tree_caption"],
				FullName = (string)x["full_name"],
				UserName = (string)x["user_name"],
				ParentId = Convert.ToInt32(x["parent_id"]),
				Color = string.Format("rgb({0},0,0)", Convert.ToInt32(x["font_color"])),
				IsBold = Convert.ToInt32(x["font_style"]) == 0,
				Comment = x["comments"] == DBNull.Value ? "" : (string)x["comments"],
				Status = (string)x["status_name"],
				StatusId = Convert.ToInt32(x["t1003_id"]),
				FromDate = Convert.ToDateTime(x["fd"]),
				ToDate = Convert.ToDateTime(x["td"]),
				CreateDate = table.Columns.Contains("fd1") && x["fd1"] != DBNull.Value ? Convert.ToDateTime(x["fd1"]) : null as DateTime?,
				NewsDays = Convert.ToInt32(x["news_days"]),
				RootId = Convert.ToInt32(x["root_id"])
			});

			return treeItems.ToList();
		}

		public IEnumerable<Tree> GetTreeForLink(int treeId, decimal userId, int languageId) {
			var cmd = _context.CreateCommandText("pg_faq_select.get_faq_tree_for_link(:p_tree_id, :p_user_id, :p_record_count);");
			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.RefCursor, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_tree_id", OracleDbType.Decimal, treeId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_record_count", OracleDbType.Decimal, null, ParameterDirection.Input));

			var table = _context.GetTable(cmd, oracleParams.ToArray());

			var treeItems = table.AsEnumerable().Select(x => new Tree {
				Id = Convert.ToInt32(x["t1101_id"]),
				Text = (string)x["tree_caption"],
				ParentId = Convert.ToInt32(x["parent_id"]),
				Color = string.Format("rgb({0},0,0)", Convert.ToInt32(x["font_color"])),
				IsBold = Convert.ToInt32(x["font_style"]) == 0,
				Comment = x["comments"] == DBNull.Value ? "" : (string)x["comments"],
				Status = (string)x["status_name"],
				StatusId = Convert.ToInt32(x["t1003_id"]),
				FromDate = Convert.ToDateTime(x["fd"]),
				ToDate = Convert.ToDateTime(x["td"]),
				CreateDate = table.Columns.Contains("fd1") && x["fd1"] != DBNull.Value ? Convert.ToDateTime(x["fd1"]) : null as DateTime?,
				NewsDays = Convert.ToInt32(x["news_days"])
			});

			return treeItems.ToList();
		}

		public IEnumerable<Sms> GetTreeSms(int treeId, DateTime date, int langId, decimal userId) {

			var cmd = "begin :data := pg_faq_select.get_faq_sms_list(:p_tree_id, :p_group_name, :p_date, :p_lang_id, :p_user_id, :p_record_count); end;";
			var table = _context.GetTable(cmd, new OracleParameter("data", OracleDbType.RefCursor, ParameterDirection.ReturnValue),
																				 new OracleParameter("p_tree_id", OracleDbType.Decimal, treeId, ParameterDirection.Input),
																				 new OracleParameter("p_group_name", OracleDbType.Char, null, ParameterDirection.Input),
																				 new OracleParameter("p_date", OracleDbType.Date, date, ParameterDirection.Input),
																				 new OracleParameter("p_lang_id", OracleDbType.Decimal, langId, ParameterDirection.Input),
																				 new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input),
																				 new OracleParameter("p_record_count", OracleDbType.Decimal, null, ParameterDirection.Input)
			);
			
			var result = table.AsEnumerable().Select(x => new Sms {
				Id = Convert.ToInt32(x["t1101_id"]),
				Comment = x["comments"] == DBNull.Value ? "" : (string)x["comments"],
				SortOrder = x["position"] == DBNull.Value ? 0 : Convert.ToInt32(x["position"]),
				GroupName = (string)x["group_name"],
				Text = (string)x["sms_text"]
			});

			return result;
		}

		public Answer GetTreeAnswer(int treeId, DateTime date, int langId, decimal userId) {
			var cmd = _context.CreateCommandText("pg_faq_select.get_faq_answers_list(:p_tree_id, :p_date, :p_lang_id, :p_user_id, :p_record_count);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.RefCursor, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_tree_id", OracleDbType.Decimal, treeId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_date", OracleDbType.Date, date, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_lang_id", OracleDbType.Decimal, langId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_record_count", OracleDbType.Decimal, null, ParameterDirection.Output));

			var result = _context.GetTable(cmd, oracleParams.ToArray());

			var templates = result.AsEnumerable().Select(x => new Answer {
				AnswerId = Convert.ToInt32(x["t1102_id"]),
				FileName = x["answer_link"] == DBNull.Value ? "" : (string)x["answer_link"],
				FromDate = Convert.ToDateTime(x["start_date"]),
				ToDate = Convert.ToDateTime(x["stop_date"]),
				Comment = x["comments"] == DBNull.Value ? "" : (string)x["comments"],
			});

			return templates.LastOrDefault();
		}

		public string GetTreeAnswerName(int treeId, DateTime date, int langId, decimal userId) {

			var cmd = _context.CreateCommandText("pg_faq_select.get_faq_answers_list(:p_tree_id, :p_date, :p_lang_id, :p_user_id, :p_record_count);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.RefCursor, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_tree_id", OracleDbType.Decimal, treeId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_date", OracleDbType.Date, date, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_lang_id", OracleDbType.Decimal, langId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_record_count", OracleDbType.Decimal, null, ParameterDirection.Output));

			var result = _context.GetTable(cmd, oracleParams.ToArray());

			var templateName = string.Empty;
			var templates = result.AsEnumerable().Select(x => new {
				name = x["answer_link"] == DBNull.Value ? "" : (string)x["answer_link"]
			});
			if(templates != null && templates.Count() > 0) {
				var template = templates.LastOrDefault();
				if(template != null) {
					templateName = template.name;
				}
			}

			return templateName;
		}

		public IEnumerable<Faq> GetFaqTreeFavourite(int languageId, decimal userId) {
			var cmd = "begin :data := pg_faq_select.get_faq_tree_custom(:p_lang_id, :p_user_id, :p_record_count); end;";

			var table = _context.GetTable(cmd, new OracleParameter("data", OracleDbType.RefCursor, ParameterDirection.ReturnValue),
																						new OracleParameter("p_lang_id", OracleDbType.Decimal, languageId, ParameterDirection.Input),
																						new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input),
																						new OracleParameter("p_record_count", OracleDbType.Decimal, null, ParameterDirection.Input)
			);

			var ssss = table.AsEnumerable().ToList();

			return table.AsEnumerable().Select(x => new Faq {
				Id = Convert.ToInt32(x["t1101_id"]),
				RecordId = Convert.ToInt32(x["t1105_id"]),
				Name = (string)x["tree_caption"]
			});
		}

		public IEnumerable<Faq> GetFaqTreeTop10(int languageId, decimal userId) {
			var cmd = "begin :data := pg_faq_select.get_faq_tree_top10(:p_lang_id, :p_user_id, :p_record_count); end;";
			var table = _context.GetTable(cmd,
																																																																			new OracleParameter("data", OracleDbType.RefCursor, ParameterDirection.ReturnValue),
																																																																			new OracleParameter("p_lang_id", OracleDbType.Decimal, languageId, ParameterDirection.Input),
																																																																			new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input),
																																																																			new OracleParameter("p_record_count", OracleDbType.Decimal, null, ParameterDirection.Input)
			);

			return table.AsEnumerable().Select(x => new Faq {
				Id = Convert.ToInt32(x["t1101_id"]),
				Name = (string)x["tree_caption"]
			});
		}

		public IEnumerable<Faq> GetFaqTreeTop10Month(int languageId, decimal userId) {
			var cmd = "begin :data := pg_faq_select.get_faq_tree_last10(:p_lang_id, :p_user_id, :p_record_count); end;";
			var table = _context.GetTable(cmd,
																																																																			new OracleParameter("data", OracleDbType.RefCursor, ParameterDirection.ReturnValue),
																																																																			new OracleParameter("p_lang_id", OracleDbType.Decimal, languageId, ParameterDirection.Input),
																																																																			new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input),
																																																																			new OracleParameter("p_record_count", OracleDbType.Decimal, null, ParameterDirection.Input)
			);

			return table.AsEnumerable().Select(x => new Faq {
				Id = Convert.ToInt32(x["t1101_id"]),
				Name = (string)x["tree_caption"]
			});
		}

		public IEnumerable<Faq> GetFaqTreeTop10Today(int languageId, decimal userId) {
			var cmd = "begin :data := pg_faq_select.get_faq_tree_top10_today(:p_lang_id, :p_user_id, :p_record_count); end;";
			var table = _context.GetTable(cmd,
																																																																			new OracleParameter("data", OracleDbType.RefCursor, ParameterDirection.ReturnValue),
																																																																			new OracleParameter("p_lang_id", OracleDbType.Decimal, languageId, ParameterDirection.Input),
																																																																			new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input),
																																																																			new OracleParameter("p_record_count", OracleDbType.Decimal, null, ParameterDirection.Input)
			);

			return table.AsEnumerable().Select(x => new Faq {
				Id = Convert.ToInt32(x["t1101_id"]),
				Name = (string)x["tree_caption"]
			});
		}

		public IEnumerable<Faq> GetFaqTreeTopCustomerQuestions(string msisdn, int languageId, decimal userId) {
			var cmd = _context.CreateCommandText("pg_faq_select.get_faq_tree_msisdn(:p_msisdn, :p_lang_id, :p_user_id, :p_record_count);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.RefCursor, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_msisdn", OracleDbType.NVarchar2, msisdn, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_lang_id", OracleDbType.Decimal, languageId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_record_count", OracleDbType.Decimal, null, ParameterDirection.Input));

			var table = _context.GetTable(cmd, oracleParams.ToArray());

			return table.AsEnumerable().Select(x => new Faq {
				Id = Convert.ToInt32(x["t1101_id"]),
				Name = (string)x["tree_caption"]
			});
		}

		public void AddSmsToTree(int treeId, int? groupId, string groupName, string sms, string comment, int position, decimal userId, int languageId) {
			var cmd = _context.CreateCommandText("pg_faq.mod_faq_sms(:p_id, :p_fd, :p_td, :p_faq_tree_id, :p_lang_id, :p_sms_text, :p_group_name, :p_position, :p_user_id, :p_comments, :vpg, :vfn, :verrdsc, :voraerror);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.Decimal, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_id", OracleDbType.Decimal, -1, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_fd", OracleDbType.Date, null, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_td", OracleDbType.Date, null, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_faq_tree_id", OracleDbType.Decimal, treeId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_lang_id", OracleDbType.Decimal, languageId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_sms_text", OracleDbType.Varchar2, sms, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_group_name", OracleDbType.Varchar2, groupName, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_position", OracleDbType.Decimal, position, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_comments", OracleDbType.Varchar2, comment, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("vpg", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("vfn", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("verrdsc", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("voraerror", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));

			var result = _context.ExecuteCommand(cmd, oracleParams);
		}

		public void RemoveSmsFromTree(int treeId, int? groupId, string groupName, string sms, string comment, decimal userId, int languageId) {
			var cmd = _context.CreateCommandText("pg_faq_ws.del_faq_sms_ws(:p_id, :p_fd, :p_faq_tree_id, :p_lang_id, :p_group_name, :p_user_id, :p_comments, :vpg, :vfn, :verrdsc, :voraerror);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.Decimal, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_id", OracleDbType.Decimal, -1, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_fd", OracleDbType.Date, null, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_faq_tree_id", OracleDbType.Decimal, treeId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_lang_id", OracleDbType.Decimal, languageId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_group_name", OracleDbType.Varchar2, groupName, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_comments", OracleDbType.Varchar2, comment, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("vpg", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("vfn", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("verrdsc", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("voraerror", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));

			var result = _context.ExecuteCommand(cmd, oracleParams);
		}

		public bool MoveNodeUp(int nodeId, int parentId, decimal userId, string comment) {
			var cmd = _context.CreateCommandText("pg_faq_ws.move_tree_up_down_ws(:p_tree_id, :p_parent_id, :p_dir, :p_user_id, :p_comments, :vpg, :vfn, :verrdsc, :voraerror);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.Decimal, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_tree_id", OracleDbType.Decimal, nodeId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_parent_id", OracleDbType.Decimal, parentId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_dir", OracleDbType.Decimal, 1, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_comments", OracleDbType.Varchar2, comment, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("vpg", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("vfn", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("verrdsc", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("voraerror", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));

			var result = _context.ExecuteCommand(cmd, oracleParams);

			return result != null;
		}

		public bool MoveNodeDown(int nodeId, int parentId, decimal userId, string comment) {
			var cmd = _context.CreateCommandText("pg_faq_ws.move_tree_up_down_ws(:p_tree_id, :p_parent_id, :p_dir, :p_user_id, :p_comments, :vpg, :vfn, :verrdsc, :voraerror);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.Decimal, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_tree_id", OracleDbType.Decimal, nodeId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_parent_id", OracleDbType.Decimal, parentId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_dir", OracleDbType.Decimal, -1, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_comments", OracleDbType.Varchar2, comment, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("vpg", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("vfn", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("verrdsc", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("voraerror", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));

			var result = _context.ExecuteCommand(cmd, oracleParams);

			return result != null;
		}

		public bool MoveInPreActive(int nodeId, int parentId, decimal userId, string comment) {

			var cmd = _context.CreateCommandText("pg_faq_ws.set_faq_tree_preactive_ws(:p_fd, :p_tree_id, :p_lang_id, :p_user_id, :p_comments, :vpg, :vfn, :verrdsc, :voraerror);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.Decimal, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_fd", OracleDbType.Date, DateTime.Now, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_tree_id", OracleDbType.Decimal, nodeId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_lang_id", OracleDbType.Decimal, 1, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_comments", OracleDbType.Varchar2, comment, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("vpg", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("vfn", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("verrdsc", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("voraerror", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));

			var result = _context.ExecuteCommand(cmd, oracleParams);

			return result != null;
		}

		public bool AddNodeToFavourite(int nodeId, decimal userId, string comment) {
			var cmd = _context.CreateCommandText("pg_faq_ws.mod_user_custom_tree_ws(:p_id, :p_users_list_id, :p_faq_tree_id, :p_add_type, :p_user_id, :p_comments, :vpg, :vfn, :verrdsc, :voraerror);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.Decimal, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_id", OracleDbType.Decimal, -1, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_users_list_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_faq_tree_id", OracleDbType.Decimal, nodeId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_add_type", OracleDbType.Decimal, 1, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_comments", OracleDbType.Varchar2, comment, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("vpg", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("vfn", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("verrdsc", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("voraerror", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));

			var result = _context.ExecuteCommand(cmd, oracleParams);

			return result != null;
		}

		public bool RemoveNodeFromFavourite(int recordId, int nodeId, decimal userId, string comment) {
			var cmd = _context.CreateCommandText("pg_faq_ws.mod_user_custom_tree_ws(:p_id, :p_users_list_id, :p_faq_tree_id, :p_add_type, :p_user_id, :p_comments, :vpg, :vfn, :verrdsc, :voraerror);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.Decimal, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_id", OracleDbType.Decimal, recordId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_users_list_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_faq_tree_id", OracleDbType.Decimal, nodeId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_add_type", OracleDbType.Decimal, 2, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_comments", OracleDbType.Varchar2, comment, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("vpg", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("vfn", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("verrdsc", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("voraerror", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));

			var result = _context.ExecuteCommand(cmd, oracleParams);

			return result != null;
		}

		public bool RemoveTreeNode(int nodeId, decimal userId) {
			var cmd = _context.CreateCommandText("pg_faq_ws.del_tree_ws(:p_tree_id, :p_user_id, :p_comments, :vpg, :vfn, :verrdsc, :voraerror);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.Decimal, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_tree_id", OracleDbType.Decimal, nodeId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_comments", OracleDbType.Varchar2, null, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("vpg", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("vfn", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("verrdsc", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("voraerror", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));

			var result = _context.ExecuteCommand(cmd, oracleParams);
			return result != null;
		}

		public List<Tree> GetFaqTreeForChange(int treeId, int languageId, decimal userId) {
			return GetFaqTree(new DateTime(1900, 1, 1), true, QuestionnaireContext.Current.LanguageId, QuestionnaireContext.Current.UserId);
		}

		public List<Tree> GetFaqTreeForCopy(int treeId, int languageId, decimal userId) {
			var cmd = _context.CreateCommandText("pg_faq_select.get_faq_tree_for_copy(:p_tree_id, :p_user_id, :p_record_count);");
			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.RefCursor, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_tree_id", OracleDbType.Decimal, treeId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_record_count", OracleDbType.Decimal, null, ParameterDirection.Input));

			var table = _context.GetTable(cmd, oracleParams.ToArray());

			var treeItems = table.AsEnumerable().Select(x => new Tree {
				Id = Convert.ToInt32(x["t1101_id"]),
				Text = (string)x["tree_caption"],
				ParentId = Convert.ToInt32(x["parent_id"]),
				Color = string.Format("rgb({0},0,0)", Convert.ToInt32(x["font_color"])),
				IsBold = Convert.ToInt32(x["font_style"]) == 0,
				Comment = x["comments"] == DBNull.Value ? "" : (string)x["comments"],
				Status = (string)x["status_name"],
				StatusId = Convert.ToInt32(x["t1003_id"]),
				FromDate = Convert.ToDateTime(x["fd"]),
				ToDate = Convert.ToDateTime(x["td"]),
				CreateDate = table.Columns.Contains("fd1") && x["fd1"] != DBNull.Value ? Convert.ToDateTime(x["fd1"]) : null as DateTime?,
				NewsDays = Convert.ToInt32(x["news_days"])
			});

			return treeItems.ToList();
		}

		public decimal AddTreeNode(int nodeId, string text, int statusId, DateTime fromDate, DateTime toDate, int newsDays, string comment, decimal userId) {
			var cmd = _context.CreateCommandText("pg_faq_ws.mod_faq_tree_ws(:p_id, :p_fd, :p_td, :p_parent_id, :p_tree_caption, :p_start_date, :p_stop_date, :p_news_days, :p_is_important, :p_position, :p_status_id, :p_user_id, :p_comments, :vpg, :vfn, :verrdsc, :voraerror);");
			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.Decimal, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_id", OracleDbType.Decimal, -1, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_fd", OracleDbType.Date, fromDate, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_td", OracleDbType.Date, toDate, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_parent_id", OracleDbType.Decimal, nodeId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_tree_caption", OracleDbType.Varchar2, text, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_start_date", OracleDbType.Date, fromDate, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_stop_date", OracleDbType.Date, toDate, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_news_days", OracleDbType.Decimal, newsDays, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_is_important", OracleDbType.Char, "N", ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_position", OracleDbType.Decimal, null, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_status_id", OracleDbType.Decimal, statusId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_comments", OracleDbType.Varchar2, comment, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("vpg", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("vfn", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("verrdsc", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("voraerror", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			var result = _context.ExecuteCommand(cmd, oracleParams);

			if(oracleParams[0] != null && oracleParams[0].Value != null) {
				var stringResponse = oracleParams[0].Value.ToString();

				if(!string.IsNullOrEmpty(stringResponse)) {
					var response = Convert.ToDecimal(stringResponse);
					return response;
				}
			}

			return 0;

			//return result != null;
		}

		public bool ChangeTreeNode(int nodeId, int parentId, string text, int statusId, DateTime fromDate, DateTime toDate, int newsDays, string comment, decimal userId) {
			var cmd = _context.CreateCommandText("pg_faq_ws.mod_faq_tree_ws(:p_id, :p_fd, :p_td, :p_parent_id, :p_tree_caption, :p_start_date, :p_stop_date, :p_news_days, :p_is_important, :p_position, :p_status_id, :p_user_id, :p_comments, :vpg, :vfn, :verrdsc, :voraerror);");
			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.Decimal, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_id", OracleDbType.Decimal, nodeId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_fd", OracleDbType.Date, fromDate, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_td", OracleDbType.Date, toDate, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_parent_id", OracleDbType.Decimal, parentId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_tree_caption", OracleDbType.Varchar2, text, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_start_date", OracleDbType.Date, fromDate, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_stop_date", OracleDbType.Date, toDate, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_news_days", OracleDbType.Decimal, newsDays, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_is_important", OracleDbType.Char, "N", ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_position", OracleDbType.Decimal, null, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_status_id", OracleDbType.Decimal, statusId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_comments", OracleDbType.Varchar2, comment, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("vpg", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("vfn", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("verrdsc", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("voraerror", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			var result = _context.ExecuteCommand(cmd, oracleParams);
			return result != null;
		}

		public bool CopyTreeNode(int nodeId, int copyToTreeId, bool copyWithChildren, string text, int statusId, DateTime fromDate, DateTime toDate, int newsDays, string comment, decimal userId) {
			var cmd = _context.CreateCommandText("pg_faq_ws.copy_tree_ws(:p_tree_id, :p_new_parent_id, :p_copy_type, :p_with_children, :p_user_id, :p_comments, :vpg, :vfn, :verrdsc, :voraerror);");
			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.Decimal, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_tree_id", OracleDbType.Decimal, nodeId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_new_parent_id", OracleDbType.Decimal, copyToTreeId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_copy_type", OracleDbType.Decimal, 2, ParameterDirection.Input)); //TODO: 2 is for copy
			oracleParams.Add(new OracleParameter("p_with_children", OracleDbType.NVarchar2, copyWithChildren ? "Y" : "N", ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_comments", OracleDbType.NVarchar2, comment, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("vpg", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("vfn", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("verrdsc", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("voraerror", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			var result = _context.ExecuteCommand(cmd, oracleParams);
			return result != null;
		}

		public bool MoveTreeNode(int nodeId, int copyToTreeId, string text, int statusId, DateTime fromDate, DateTime toDate, int newsDays, string comment, decimal userId) {
			var cmd = _context.CreateCommandText("pg_faq_ws.copy_tree_ws(:p_tree_id, :p_new_parent_id, :p_copy_type, :p_with_children, :p_user_id, :p_comments, :vpg, :vfn, :verrdsc, :voraerror);");
			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.Decimal, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_tree_id", OracleDbType.Decimal, nodeId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_new_parent_id", OracleDbType.Decimal, copyToTreeId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_copy_type", OracleDbType.Decimal, 1, ParameterDirection.Input)); //TODO: 1 is for move
			oracleParams.Add(new OracleParameter("p_with_children", OracleDbType.NVarchar2, "N", ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_comments", OracleDbType.NVarchar2, comment, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("vpg", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("vfn", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("verrdsc", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("voraerror", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			var result = _context.ExecuteCommand(cmd, oracleParams);
			return result != null;
		}

		public bool AddEmailAnswer(int nodeId, int answerId, DateTime fromdate, DateTime toDate, string answer, string htmlName, List<string> searchTags, string comment, bool saveAsTemplate, string templateName, decimal userId, int languageId) {
			var cmd = _context.CreateCommandText("pg_faq_ws.mod_faq_answers_ws(:p_id, :p_fd, :p_td,:p_faq_tree_id, :p_lang_id, :p_answer_link, :p_answer_blob_name, :p_answer_blob_date, :p_answer_blob_type, :p_answer_blob, :p_start_date, :p_stop_date, :p_is_shablon, :p_shablon_name, :p_words_list1, :p_words_list2, :p_words_list3, :p_words_list4, :p_words_list5, :p_words_list6, :p_words_list7, :p_words_list8, :p_words_list9, :p_user_id, :p_comments, :vpg, :vfn, :verrdsc, :voraerror);");
			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.Decimal, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_id", OracleDbType.Decimal, answerId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_fd", OracleDbType.Date, fromdate, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_td", OracleDbType.Date, toDate, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_faq_tree_id", OracleDbType.Decimal, nodeId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_lang_id", OracleDbType.Decimal, languageId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_answer_link", OracleDbType.Varchar2, htmlName, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_answer_blob_name", OracleDbType.Varchar2, null, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_answer_blob_date", OracleDbType.Date, DateTime.Now, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_answer_blob_type", OracleDbType.NVarchar2, string.Empty, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_answer_blob", OracleDbType.Blob, null, ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("p_start_date", OracleDbType.Date, fromdate, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_stop_date", OracleDbType.Date, toDate, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_is_shablon", OracleDbType.NVarchar2, saveAsTemplate ? "Y" : "N", ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_shablon_name", OracleDbType.NVarchar2, templateName, ParameterDirection.Input));

			oracleParams.Add(new OracleParameter("p_words_list1", OracleDbType.Varchar2, searchTags.Count > 0 ? searchTags[0] : string.Empty, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_words_list2", OracleDbType.Varchar2, searchTags.Count > 1 ? searchTags[1] : string.Empty, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_words_list3", OracleDbType.Varchar2, searchTags.Count > 2 ? searchTags[2] : string.Empty, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_words_list4", OracleDbType.Varchar2, searchTags.Count > 3 ? searchTags[3] : string.Empty, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_words_list5", OracleDbType.Varchar2, searchTags.Count > 4 ? searchTags[4] : string.Empty, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_words_list6", OracleDbType.Varchar2, searchTags.Count > 5 ? searchTags[5] : string.Empty, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_words_list7", OracleDbType.Varchar2, searchTags.Count > 6 ? searchTags[6] : string.Empty, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_words_list8", OracleDbType.Varchar2, searchTags.Count > 7 ? searchTags[7] : string.Empty, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_words_list9", OracleDbType.Varchar2, searchTags.Count > 8 ? searchTags[8] : string.Empty, ParameterDirection.Input));

			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_comments", OracleDbType.NVarchar2, comment, ParameterDirection.Input));

			oracleParams.Add(new OracleParameter("vpg", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("vfn", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("verrdsc", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("voraerror", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			var result = _context.ExecuteCommand(cmd, oracleParams);
			return result != null;
		}

		public List<Tree> SearchInTree(int searchType, string searchTerm, string searchComment, bool onlyPublished, DateTime date, DateTime periodStart, DateTime periodEnd, int statusId, int ownerId, decimal userId, int languageId) {
			var cmd = _context.CreateCommandText("pg_faq_select.get_found_data(:p_search_type, :p_words, :p_only_published, :p_date, :p_period_start, :p_period_srop, :p_status_id, :p_comment, :p_owner_id, :p_lang_id, :p_user_id, :p_record_count);");

			var isExtendedSearch = searchType == 0;

			var _onlyPublished = isExtendedSearch ? (onlyPublished ? "Y" : "N") : "Y";
			DateTime? _date = isExtendedSearch ? (DateTime?)date : date;
			DateTime? _periodStart = isExtendedSearch ? (DateTime?)periodStart : null;
			DateTime? _periodEnd = isExtendedSearch ? (DateTime?)periodEnd : null;
			var _statusId = isExtendedSearch ? statusId : 1;
			var _ownerId = isExtendedSearch ? ownerId : 1;

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.RefCursor, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_search_type", OracleDbType.Decimal, searchType, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_words", OracleDbType.NVarchar2, searchTerm, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_only_published", OracleDbType.NVarchar2, _onlyPublished, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_date", OracleDbType.Date, _date, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_period_start", OracleDbType.Date, _periodStart, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_period_srop", OracleDbType.Date, _periodEnd, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_status_id", OracleDbType.Decimal, _statusId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_comment", OracleDbType.NVarchar2, searchComment, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_owner_id", OracleDbType.Decimal, _ownerId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_lang_id", OracleDbType.Decimal, languageId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_record_count", OracleDbType.Decimal, null, ParameterDirection.Output));
			var table = _context.GetTable(cmd, oracleParams.ToArray());

			var treeItems = table.AsEnumerable().Select(x => new Tree {
				Id = Convert.ToInt32(x["t1101_id"]),
				Text = (string)x["tree_caption"],
				ParentId = Convert.ToInt32(x["parent_id"]),
				Color = string.Format("rgb({0},0,0)", Convert.ToInt32(x["font_color"])),
				IsBold = Convert.ToInt32(x["font_style"]) == 0,
				Comment = x["comments"] == DBNull.Value ? "" : (string)x["comments"],
				Status = (string)x["status_name"],
				StatusId = Convert.ToInt32(x["t1003_id"]),
				FromDate = Convert.ToDateTime(x["fd"]),
				ToDate = Convert.ToDateTime(x["td"]),
				CreateDate = table.Columns.Contains("fd1") && x["fd1"] != DBNull.Value ? Convert.ToDateTime(x["fd1"]) : null as DateTime?,
				NewsDays = Convert.ToInt32(x["news_days"])
			});

			return treeItems.OrderBy(x => x.ParentId).ToList();
		}

		public IEnumerable<AnswerTemplate> GetTreeEmailTemlpates(int treeId) {
			var cmd = _context.CreateCommandText("pg_faq_select.get_faq_answer_shablons_list(:p_tree_id, :p_lang_id, :p_user_id, :p_record_count);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.RefCursor, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_tree_id", OracleDbType.Decimal, treeId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_lang_id", OracleDbType.Decimal, 1, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, 183, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_record_count", OracleDbType.Decimal, null, ParameterDirection.Output));

			var result = _context.GetTable(cmd, oracleParams.ToArray());

			var templateName = string.Empty;
			var templates = result.AsEnumerable().Select(x => new AnswerTemplate {
				Id = Convert.ToInt32(x["t1101_id"]),
				FileName = x["answer_link"] == DBNull.Value ? "" : (string)x["answer_link"],
				Name = x["caption"] == DBNull.Value ? "" : (string)x["caption"],
			});

			return templates;
		}

		public IEnumerable<Owner> GetOwners(decimal userId, int languageId) {
			var cmd = _context.CreateCommandText("pg_faq_select.get_users_list(:p_user_id, :p_record_count);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.RefCursor, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_record_count", OracleDbType.Decimal, null, ParameterDirection.Output));

			var result = _context.GetTable(cmd, oracleParams.ToArray());

			var owners = result.AsEnumerable().Select(x => new Owner {
				Id = Convert.ToInt32(x["t0704_id"]),
				Name = x["person"] == DBNull.Value ? "" : (string)x["person"],
			});

			return owners;
		}

		public IEnumerable<AnswerVersion> GetAnswerVersions(int nodeId, decimal userId, int languageId) {
			var cmd = _context.CreateCommandText("pg_faq_select.get_answer_versions(:p_tree_id, :p_lang_id, :p_user_id, :p_record_count);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.RefCursor, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_tree_id", OracleDbType.Decimal, nodeId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_lang_id", OracleDbType.Decimal, languageId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_record_count", OracleDbType.Decimal, null, ParameterDirection.Output));

			var result = _context.GetTable(cmd, oracleParams.ToArray());

			var versions = result.AsEnumerable().Select(x => new AnswerVersion {
				Id = Convert.ToInt32(x["t1102_id"]),
				FromDate = Convert.ToDateTime(x["in_date"]),
				ToDate = Convert.ToDateTime(x["td"]),
				FileName = x["answer_link"] == DBNull.Value ? "" : (string)x["answer_link"],
				Username = x["user_name"] == DBNull.Value ? "" : (string)x["user_name"],
				Comment = x["comments"] == DBNull.Value ? "" : (string)x["comments"]
			});

			return versions;
		}

		public IEnumerable<DeletedTree> GetFaqTreeDeleted(decimal userId, int languageId) {
			var cmd = _context.CreateCommandText("pg_faq_select.get_faq_tree_deleted(:p_user_id, :p_record_count);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.RefCursor, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_record_count", OracleDbType.Decimal, null, ParameterDirection.Output));

			var result = _context.GetTable(cmd, oracleParams.ToArray());

			var treeItems = result.AsEnumerable().Select(x => new DeletedTree {
				Id = Convert.ToInt32(x["t1101_id"]),
				Caption = (string)x["tree_caption"],
				FromDate = Convert.ToDateTime(x["fd"]),
				ToDate = Convert.ToDateTime(x["GREATEST(T.TD,T.STOP_DATE)"]),
				Username = x["user_name"] == DBNull.Value ? "" : (string)x["user_name"],
				Comments = x["comments"] == DBNull.Value ? "" : (string)x["comments"],
				IsNew = (string)x["is_new"] == "N",
				DeleteType = (string)x["delete_type"]
			});

			return treeItems;
		}

		public bool RecoverTreeFromDeletedRecords(int nodeId, string comment, decimal userId, int languageId) {
			var cmd = _context.CreateCommandText("pg_faq_ws.restore_tree_ws(:p_tree_id, :p_user_id, :p_comments, :vpg, :vfn, :verrdsc, :voraerror);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.Decimal, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_tree_id", OracleDbType.Decimal, nodeId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_comments", OracleDbType.Varchar2, comment, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("vpg", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("vfn", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("verrdsc", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("voraerror", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));

			var result = _context.ExecuteCommand(cmd, oracleParams);
			return result != null;
		}

		public IEnumerable<Reminder> GetReminderItems(decimal userId, int languageId) {
			var cmd = _context.CreateCommandText("pg_faq_select.get_faq_tree_reminder(:p_user_id, :p_record_count);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.RefCursor, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_record_count", OracleDbType.Decimal, null, ParameterDirection.Output));

			var result = _context.GetTable(cmd, oracleParams.ToArray());

			var reminderItems = result.AsEnumerable().Select(x => new Reminder {
				Id = Convert.ToInt32(x["t1101_id"]),
				Caption = (string)x["tree_caption"],
				FromDate = Convert.ToDateTime(x["fd"]),
				ToDate = Convert.ToDateTime(x["td"]),
				InDate = Convert.ToDateTime(x["in_date"]),
				User = x["user_name"] == DBNull.Value ? "" : (string)x["user_name"],
				Comments = x["comments"] == DBNull.Value ? "" : (string)x["comments"],
				Remind = Convert.ToDecimal(x["remind"]),
			});

			return reminderItems;
		}

		public IEnumerable<string> GetEmailGroups(string number, decimal userId, int languageId) {
			var cmd = _context.CreateCommandText("pg_faq_select.get_msisdn_emails_list(:p_msisdn, :p_user_id, :p_record_count);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.RefCursor, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_msisdn", OracleDbType.Decimal, number, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_record_count", OracleDbType.Decimal, null, ParameterDirection.Output));

			var result = _context.GetTable(cmd, oracleParams.ToArray());

			var reminderItems = result.AsEnumerable().Select(x => (string)x["email"]);

			return reminderItems;
		}

		public bool RegisterMsisdn(int id, int treeId, string number, string comments, decimal userId, int languageId) {
			var cmd = _context.CreateCommandText("pg_faq_ws.mod_answered_tree_ws(:p_id, :p_faq_tree_id, :p_lang_id, :p_msisdn, :p_user_id, :p_comments, :vpg, :vfn, :verrdsc, :voraerror);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.Decimal, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_id", OracleDbType.Decimal, id, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_faq_tree_id", OracleDbType.Decimal, treeId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_lang_id", OracleDbType.Decimal, languageId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_msisdn", OracleDbType.Varchar2, number, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_comments", OracleDbType.Varchar2, comments, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("vpg", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("vfn", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("verrdsc", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("voraerror", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));

			var result = _context.ExecuteCommand(cmd, oracleParams);
			return result != null;
		}

		public void SendNotificationLog(int type, int treeId, string msisdn, string email, string sms, decimal userId, int languageId) {
			var cmd = _context.CreateCommandText("pg_faq_ws.add_sent_item_ws(:p_id, :p_faq_tree_id, :p_item_type, :p_lang_id, :p_msisdn, :p_email, :p_sent_item_id, :p_user_id, :p_comments, :vpg, :vfn, :verrdsc, :voraerror);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.Decimal, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_id", OracleDbType.Decimal, -1, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_faq_tree_id", OracleDbType.Decimal, treeId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_item_type", OracleDbType.Decimal, type, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_lang_id", OracleDbType.Decimal, languageId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_msisdn", OracleDbType.Varchar2, msisdn, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_email", OracleDbType.Varchar2, email, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_sent_item_id", OracleDbType.Decimal, treeId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_comments", OracleDbType.NVarchar2, sms, ParameterDirection.Input));

			oracleParams.Add(new OracleParameter("vpg", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("vfn", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("verrdsc", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("voraerror", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));

			var result = _context.ExecuteCommand(cmd, oracleParams);
		}

		public IEnumerable<EmailComparisonFile> GetEmailComparisonFiles(int treeId, int answerId, DateTime date, decimal userId, int languageId) {
			var cmd = _context.CreateCommandText("pg_faq_select.get_faq_attachments(:p_tree_id, :p_answer_id, :p_date, :p_lang_id, :p_user_id, :p_record_count);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.RefCursor, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_tree_id", OracleDbType.Decimal, treeId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_answer_id", OracleDbType.Decimal, answerId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_date", OracleDbType.Date, date, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_lang_id", OracleDbType.Decimal, languageId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_record_count", OracleDbType.Decimal, null, ParameterDirection.Output));

			var result = _context.GetTable(cmd, oracleParams.ToArray());

			var filesResult = result.AsEnumerable().Select(x => new EmailComparisonFile {
				Id = Convert.ToInt32(x["t1109_id"]),
				FolderName = (string)x["file_link"],
				Comment = x["comments"] == DBNull.Value ? "" : (string)x["comments"],
				TreeId = treeId
			}).ToList();

			return filesResult;
		}

		public void SaveTreeFile(int id, DateTime fromDate, DateTime toDate, int treeId, int answerId, string fileName, string comment, decimal userId, int languageId) {
			var cmd = _context.CreateCommandText("pg_faq_ws.mod_tree_attachments_ws(:p_id, :p_fd, :p_td, :p_faq_tree_id, :p_answer_id, :p_lang_id, :p_link, :p_blob_name, :p_blob_dsc, :p_blob_caption, :p_blob_date, :p_blob_type, :p_blob, :p_user_id, :p_comments, :vpg, :vfn, :verrdsc, :voraerror);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.Decimal, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_id", OracleDbType.Decimal, id, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_fd", OracleDbType.Date, null, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_td", OracleDbType.Date, null, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_faq_tree_id", OracleDbType.Decimal, treeId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_answer_id", OracleDbType.Decimal, answerId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_lang_id", OracleDbType.Decimal, languageId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_link", OracleDbType.Varchar2, fileName, ParameterDirection.Input));

			oracleParams.Add(new OracleParameter("p_blob_name", OracleDbType.Varchar2, fileName, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_blob_dsc", OracleDbType.Varchar2, "", ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_blob_caption", OracleDbType.Varchar2, "", ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_blob_date", OracleDbType.Date, DateTime.Now, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_blob_type", OracleDbType.NVarchar2, string.Empty, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_blob", OracleDbType.Blob, null, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_comments", OracleDbType.NVarchar2, comment, ParameterDirection.Input));

			oracleParams.Add(new OracleParameter("vpg", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("vfn", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("verrdsc", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("voraerror", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));

			var result = _context.ExecuteCommand(cmd, oracleParams);

		}

		public void SaveTreeModalFile(int id, DateTime fromDate, DateTime toDate, int treeId, int answerId, string fileName, string comment, decimal userId, int languageId) {
			var cmd = _context.CreateCommandText("pg_faq_ws.mod_tree_attachments_ws(:p_id, :p_fd, :p_td, :p_faq_tree_id, :p_answer_id, :p_lang_id, :p_link, :p_blob_name, :p_blob_dsc, :p_blob_caption, :p_blob_date, :p_blob_type, :p_blob, :p_user_id, :p_comments, :vpg, :vfn, :verrdsc, :voraerror);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.Decimal, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_id", OracleDbType.Decimal, id, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_fd", OracleDbType.Date, null, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_td", OracleDbType.Date, null, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_faq_tree_id", OracleDbType.Decimal, treeId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_answer_id", OracleDbType.Decimal, answerId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_lang_id", OracleDbType.Decimal, languageId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_link", OracleDbType.Varchar2, fileName, ParameterDirection.Input));

			oracleParams.Add(new OracleParameter("p_blob_name", OracleDbType.Varchar2, fileName, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_blob_dsc", OracleDbType.Varchar2, "", ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_blob_caption", OracleDbType.Varchar2, "", ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_blob_date", OracleDbType.Date, DateTime.Now, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_blob_type", OracleDbType.NVarchar2, string.Empty, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_blob", OracleDbType.Blob, null, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_comments", OracleDbType.NVarchar2, comment, ParameterDirection.Input));

			oracleParams.Add(new OracleParameter("vpg", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("vfn", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("verrdsc", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("voraerror", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));

			var result = _context.ExecuteCommand(cmd, oracleParams);
		}

		public void DeleteTreeFile(int id, DateTime fromDate, string fileName, decimal userId, int languageId) {
			var cmd = _context.CreateCommandText("pg_faq_ws.del_attachment(:p_id, :p_fd, :p_user_id, :p_comments, :vpg, :vfn, :verrdsc, :voraerror);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.Decimal, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_id", OracleDbType.Decimal, id, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_fd", OracleDbType.Date, fromDate, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_comments", OracleDbType.NVarchar2, string.Empty, ParameterDirection.Input));

			oracleParams.Add(new OracleParameter("vpg", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("vfn", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("verrdsc", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("voraerror", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));

			var result = _context.ExecuteCommand(cmd, oracleParams);
		}

		public IEnumerable<GroupEmail> GetGroupEmails(decimal userId, int languageId) {
			var cmd = _context.CreateCommandText("pg_faq_select.get_group_emails_list(:p_user_id, :p_record_count);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.RefCursor, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_record_count", OracleDbType.Decimal, null, ParameterDirection.Output));

			var result = _context.GetTable(cmd, oracleParams.ToArray());

			var filesResult = result.AsEnumerable().Select(x => new GroupEmail {
				Email = (string)x["email"],
			}).ToList();

			return filesResult;
		}

		public void SendGroupEmail(int treeId, string text, int sendHour, string email, decimal userId, int languageId) {
			var cmd = _context.CreateCommandText("pg_faq_ws.add_message_to_send(:p_id, :p_fd, :p_td, :p_faq_tree_id, :p_message_type, :p_receiver, :p_message_text, :p_subject, :p_send_date, :p_send_hour, :p_user_id, :p_comments, :vpg, :vfn, :verrdsc, :voraerror);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.Decimal, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_id", OracleDbType.Decimal, -1, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_fd", OracleDbType.Date, null, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_td", OracleDbType.Date, null, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_faq_tree_id", OracleDbType.Decimal, treeId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_message_type", OracleDbType.Decimal, 1, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_receiver", OracleDbType.NVarchar2, email, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_message_text", OracleDbType.NVarchar2, text, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_subject", OracleDbType.NVarchar2, string.Empty, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_send_date", OracleDbType.Date, DateTime.Now.Date, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_send_hour", OracleDbType.Decimal, sendHour, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_comments", OracleDbType.NVarchar2, string.Empty, ParameterDirection.Input));

			oracleParams.Add(new OracleParameter("vpg", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("vfn", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("verrdsc", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("voraerror", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));

			var result = _context.ExecuteCommand(cmd, oracleParams);
		}

		public string GetCurrentCallMobileNumber(decimal userId, int languageId) {
			var cmd = _context.CreateCommandText("pg_faq_select.get_user_clipboard(p_user_id => :p_user_id);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, userId, ParameterDirection.Input));

			_context.ExecuteNonQuery(cmd, oracleParams.ToArray());
			var outputParam = oracleParams.FirstOrDefault(x => x.Direction == ParameterDirection.ReturnValue);
			if(outputParam != null && outputParam.Value != null) {
				return outputParam.Value.ToString();
			}
			return string.Empty;
		}

		public IEnumerable<TreeStatus> GetStatuses() {
			var cmd = _context.CreateCommandText("pg_faq_select.get_statuses_list;");
			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.RefCursor, ParameterDirection.ReturnValue));

			var result = _context.GetTable(cmd, oracleParams.ToArray());

			var statusesResult = result.AsEnumerable().Select(x => new TreeStatus {
				Id = Convert.ToInt32(x["t1003_id"]),
				Name = (string)x["status_name"],
			});

			return statusesResult;
		}

		#region Daemon Actions

		public IEnumerable<DaemonMessage> GetMessagesToSend() {
			var cmd = _context.CreateCommandText("pg_faq_select.get_messages_to_send(:p_type, :p_user_id, :p_record_count);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.RefCursor, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_type", OracleDbType.Decimal, 1, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, 183, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_record_count", OracleDbType.Decimal, null, ParameterDirection.Output));

			var result = _context.GetTable(cmd, oracleParams.ToArray());

			var daemonMessages = result.AsEnumerable().Select(x => new DaemonMessage {
				Id = Convert.ToInt32(x["t1113_id"]),
				MessageType = Convert.ToInt32(x["message_type"]),
				Receiver = x["receiver"] == DBNull.Value ? "" : (string)x["receiver"],
				MessageText = x["message_text"] == DBNull.Value ? "" : (string)x["message_text"],
				Subject = x["subject"] == DBNull.Value ? "" : (string)x["subject"],
				SendDate = Convert.ToDateTime(x["send_date"]),
				SendHour = Convert.ToInt32(x["send_hour"])
			});

			return daemonMessages;
		}

		public void AddMessageSend(int messageId) {
			var cmd = _context.CreateCommandText("pg_faq_ws.add_message_sent(:p_id, :p_user_id, :p_comments, :vpg, :vfn, :verrdsc, :voraerror);");

			var oracleParams = new List<OracleParameter> { };
			oracleParams.Add(new OracleParameter("data", OracleDbType.Decimal, ParameterDirection.ReturnValue));
			oracleParams.Add(new OracleParameter("p_id", OracleDbType.Decimal, messageId, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_user_id", OracleDbType.Decimal, 1, ParameterDirection.Input));
			oracleParams.Add(new OracleParameter("p_comments", OracleDbType.NVarchar2, string.Empty, ParameterDirection.Input));

			oracleParams.Add(new OracleParameter("vpg", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("vfn", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("verrdsc", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));
			oracleParams.Add(new OracleParameter("voraerror", OracleDbType.Varchar2, Utils.Utils.CommandParamSize, "", ParameterDirection.Output));

			var result = _context.ExecuteCommand(cmd, oracleParams);
		}

		#endregion

	}
}