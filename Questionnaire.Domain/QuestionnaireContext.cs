using Questionnaire.Configuration;
using Questionnaire.Domain.Entities;
using Questionnaire.Domain.Interfaces.Repositories;
using Questionnaire.Domain.Interfaces.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Questionnaire.Domain {
	[Serializable]
	public class QuestionnaireContext : IPrincipal {
		public const string CONTEXT_STORAGE_KEY = "___REQUEST_CONTEXT___";
		public const string TOKEN_KEY = "Questionnaire-Token";
		public const string LANGUAGE_KEY = "Questionnaire-Language-Code";
		public const string TIMEZONE_KEY = "Questionnaire-TimeZone";

		private ISessionRepository _sessionRepository;
		private IUserService _userService;
		private UserSession _innerSession;
		private Settings _settings;

		public static QuestionnaireContext Current {
			get { return GetStorage(CONTEXT_STORAGE_KEY) as QuestionnaireContext; }
			private set { SetStorage(CONTEXT_STORAGE_KEY, value); }
		}

		public Settings Settings {
			get {
				return _settings;
			}
		}

		public User User {
			get {
				return !_innerSession.UserId.HasValue ? null : _userService.Fetch(_innerSession.UserId.Value);
			}
			set {
				_innerSession.UserId = value == null ? null : (int?)value.Id;
			}
		}

		#region Storage Contenxt

		public static Func<IDictionary> Storage { get; set; }

		private static IDictionary GetStorage() {
			try { return Storage(); } catch { return null; }
		}

		private static object GetStorage(string key) {
			var storage = GetStorage();
			return storage == null ? null : storage[key];
		}

		private static void SetStorage(string key, object value) {
			var storage = GetStorage();
			if(storage != null) storage[key] = value;
		}

		#endregion

		#region IPrincipal

		public IIdentity Identity {
			get { return _identity ?? (_identity = new QuestionnaireContextIdentity(this)); }
		}
		private QuestionnaireContextIdentity _identity;

		public bool IsInRole(string role) {
			if(User == null || User.Roles == null) { return false; }
			return User.Roles.Any(x => x.Name.Equals(role, StringComparison.CurrentCultureIgnoreCase));
		}

		#endregion

		public static void Initialize(ISessionRepository sessionRepository, IUserService userService, Settings settings, Func<string, string> getData) {
			if(Current != null) return;

			var context = new QuestionnaireContext {
				_innerSession = sessionRepository.Fetch(getData(TOKEN_KEY)),
				_userService = userService,
				_settings = settings,
				LanguageCode = getData(LANGUAGE_KEY) ?? "en-US",
			};

			string data;

			data = getData(TIMEZONE_KEY);
			context._innerSession.TimeZoneOffSet = string.IsNullOrWhiteSpace(data) ? context._innerSession.TimeZoneOffSet : int.Parse(data);

			data = getData(LANGUAGE_KEY);
			context._innerSession.LanguageCode = string.IsNullOrWhiteSpace(data) ? context._innerSession.LanguageCode : data;

			context._sessionRepository = sessionRepository;

			Current = context;
		}

		public static void Save(Action<string, object> setData = null) {
			var context = Current;
			if(context == null) return;

			context._innerSession.LastAccess = DateTime.UtcNow;

			if(setData != null) {
				setData(TOKEN_KEY, context._innerSession.Token);
			}
			context._sessionRepository.Save(context._innerSession);
		}

		public string Token { get { return _innerSession.Token; } }

		public int TimeZoneOffSet { get { return _innerSession.TimeZoneOffSet; } }

		public string LanguageCode {
			get {
				if(string.IsNullOrEmpty(_innerSession.LanguageCode)) {
					LanguageCode = "en-US";
				}
				return Thread.CurrentThread.CurrentCulture.Name;
			}
			set {
				_innerSession.LanguageCode = value.ToLower();
				CultureInfo info = CultureInfo.GetCultureInfo(value).Clone() as CultureInfo;
				info.DateTimeFormat.ShortTimePattern = "HH:mm";
				Thread.CurrentThread.CurrentUICulture = info;
				Thread.CurrentThread.CurrentCulture = info;
			}
		}

		public int LanguageId {
			get { return 1; }
		}

		public bool IsAuthenticated { get { return _innerSession.UserId.HasValue; } }

		public decimal UserId {
			get { return _innerSession.UserId ?? 0; }
			set { _innerSession.UserId = (value == 0 ? null : (decimal?)value); }
		}

		public int SelectedNodeId {
			get {
				return _innerSession.SelectedNodeId;
			}
			set {
				_innerSession.SelectedNodeId = value;
			}
		}

		public string CallMobile {
			get {
				if(string.IsNullOrEmpty(_innerSession.CallMobile)) {
					CallMobile = string.Empty;
				}
				return _innerSession.CallMobile;
			}
			set {
				_innerSession.CallMobile = value;
			}
		}

		public string DbCallMobile {
			get {
				if(string.IsNullOrEmpty(_innerSession.DbCallMobile)) {
					DbCallMobile = string.Empty;
				}
				return _innerSession.DbCallMobile;
			}
			set {
				_innerSession.DbCallMobile = value;
			}
		}

		public bool PreActiveIsChecked {
			get {
				return _innerSession.PreActiveIsSelected;
			}
			set {
				_innerSession.PreActiveIsSelected = value;
			}
		}

		public string Username { get; set; }
	}

	public class QuestionnaireContextIdentity : IIdentity {
		private QuestionnaireContext _context;

		internal QuestionnaireContextIdentity(QuestionnaireContext context) {
			_context = context;
		}

		public string AuthenticationType {
			get { return "Questionnaire Authentication"; }
		}

		public bool IsAuthenticated {
			get {
				return _context.IsAuthenticated;
			}
		}

		public string Name {
			get {
				return _context.Username;
			}
		}
	}
}
