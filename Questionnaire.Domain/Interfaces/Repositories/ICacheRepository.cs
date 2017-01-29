using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Domain.Interfaces.Repositories {
	public interface ICacheRepository {
		bool Contains(string key);
		void Save(string key, object obj);
		void Save(int cacheMinutes, string key, object obj);
		void Remove(string key);
		object Fetch(string key);
		void RemoveUserCaches();
	}
}
