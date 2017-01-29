using Questionnaire.Configuration.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Configuration {
	public class Settings : AppSettings {
		public QuestionnaireConfig Questionnaire {
			get { return _questionnaire ?? (_questionnaire = _config.GetSection("questionnaire") as QuestionnaireConfig); }
		}
		private QuestionnaireConfig _questionnaire;
	}

	public class QuestionnaireConfig : ConfigurationSection {
		[ConfigurationProperty("newsDays", IsRequired = true)]
		public int NewsDays {
			get { return (int)this["newsDays"]; }
			set { this["newsDays"] = value; }
		}

		[ConfigurationProperty("searchTagsLength", IsRequired = true)]
		public int SearchTagsLength {
			get { return (int)this["searchTagsLength"]; }
			set { this["searchTagsLength"] = value; }
		}

		[ConfigurationProperty("slidingExpirationMinutes", IsRequired = true)]
		public int SlidingExpirationMinutes {
			get { return (int)this["slidingExpirationMinutes"]; }
			set { this["slidingExpirationMinutes"] = value; }
		}

		[ConfigurationProperty("isTestInstance", IsRequired = true)]
		public bool IsTestInstance {
			get { return (bool)this["isTestInstance"]; }
			set { this["isTestInstance"] = value; }
		}

		[ConfigurationProperty("fromEmail", IsRequired = true)]
		public string FromEmail {
			get { return (string)this["fromEmail"]; }
			set { this["fromEmail"] = value; }
		}

		[ConfigurationProperty("currentMobileNumberLoadSeconds", IsRequired = true)]
		public int CurrentMobileNumberLoadSeconds {
			get { return (int)this["currentMobileNumberLoadSeconds"]; }
			set { this["currentMobileNumberLoadSeconds"] = value; }
		}

		[ConfigurationProperty("daemonRunInterval", IsRequired = true)]
		public double DaemonRunInterval {
			get { return (double)this["daemonRunInterval"]; }
			set { this["daemonRunInterval"] = value; }
		}
	}
}
