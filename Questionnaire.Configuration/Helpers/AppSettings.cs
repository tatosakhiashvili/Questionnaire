using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Questionnaire.Configuration.Helpers {
	public class AppSettings {

		protected System.Configuration.Configuration _config;
		protected AppSettingsSection _appSettings;

		public AppSettings() {
			_config = ConfigurationManager.OpenExeConfiguration(string.Empty);
			_appSettings = _config.AppSettings;
		}

		public virtual bool IsDirty { get; set; }

		public virtual void Save() {
			if(IsDirty) lock (new object()) {
					_config.Save(ConfigurationSaveMode.Minimal, false);
					_config = ConfigurationManager.OpenExeConfiguration(string.Empty);
				}
		}

		public void BatchUpdate(IDictionary dictionary) {
			foreach(string key in dictionary.Keys) {
				SetSetting(key, dictionary[key]);
			}
			Save();
		}

		protected string GetSetting(string key, object defaultValue) {
			return _appSettings.Settings[key] != null ? _appSettings.Settings[key].Value : defaultValue.ToString();
		}

		protected T GetSetting<T>(string key, T defaultValue) {
			return _appSettings.Settings[key] != null ? (T)Convert.ChangeType(_appSettings.Settings[key].Value, typeof(T)) : defaultValue;
		}

		protected void SetSetting<T>(string key, T defaultValue) {
			IsDirty = true;
			_appSettings.Settings.Remove(key);
			_appSettings.Settings.Add(key, defaultValue.ToString());
		}

		protected void SetSetting(string key, object defaultValue) {
			IsDirty = true;
			_appSettings.Settings.Remove(key);
			_appSettings.Settings.Add(key, defaultValue.ToString());
		}

		public string ApplicationName {
			get { return GetSetting("ApplicationName", string.Empty); }
		}

		public string ApplicationVersion {
			get { return GetSetting("ApplicationVersion", "0.0.0"); }
		}

		public string GlobalResources {
			get { return GetSetting("GlobalResources", "app_GlobalResources"); }
		}

		public string ResStrings {
			get { return GetSetting("ResStrings", "Strings"); }
		}

		public string ResSiteMap {
			get { return GetSetting("ResSiteMap", "SiteMap"); }
		}

		public string ResValidators {
			get { return GetSetting("ResValidators", "Validators"); }
		}

		public SmtpSection Mail {
			get { return _mail ?? (_mail = (_config.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup).Smtp); }
		}
		private SmtpSection _mail;
	}

	public class ConfigurationTextElement<T> : ConfigurationElement {

		private T _value;

		protected override void DeserializeElement(XmlReader reader, bool serializeCollectionKey) {
			_value = (T)reader.ReadElementContentAs(typeof(T), null);
		}

		public T Value {
			get { return _value; }
		}

		public static implicit operator T(ConfigurationTextElement<T> instance) {
			return instance.Value;
		}

		public static explicit operator ConfigurationTextElement<T>(T value) {
			return new ConfigurationTextElement<T> { _value = value };
		}
	}

	public class ConfigurationCollection<T> : ConfigurationElementCollection where T : ConfigurationElement, IConfigurationKey, new() {

		public T this[int index] {
			get { return (T)BaseGet(index); }
			set { if(BaseGet(index) != null) { BaseRemoveAt(index); } BaseAdd(index, value); }
		}

		public void Add(T serviceConfig) {
			BaseAdd(serviceConfig);
		}

		public void Clear() {
			BaseClear();
		}

		protected override ConfigurationElement CreateNewElement() {
			return new T();
		}

		public void RemoveAt(int index) {
			BaseRemoveAt(index);
		}

		protected override object GetElementKey(ConfigurationElement element) {
			return (element as IConfigurationKey).Key;
		}
	}

	public interface IConfigurationKey {
		object Key { get; }
	}
}