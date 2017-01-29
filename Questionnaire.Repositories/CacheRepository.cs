using Questionnaire.Domain;
using Questionnaire.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Repositories {
	public class CacheRepository : ICacheRepository {
		private MemoryCache _cache;

		public CacheRepository() {
			_cache = MemoryCache.Default;
		}

		public bool Contains(string key) {
			return _cache.Contains(key);
		}

		public object Fetch(string key) {
			if(_cache.Contains(key)) {
				return _cache[key] as object;
			}
			return null;
		}

		public void Remove(string key) {
			_cache.Remove(key);
		}

		public void RemoveUserCaches() {
			if(QuestionnaireContext.Current.IsAuthenticated) {
				var key = string.Format("User_Caches_List_{0}", QuestionnaireContext.Current.UserId);

				if(Contains(key)) {
					var cacheKeys = Fetch(key);
					if(cacheKeys != null) {
						foreach(var cache in cacheKeys as List<string>) {
							Remove(cache);
						}
					}
					Remove(key);
				}
			}
		}

		public void Save(string key, object obj) {
			CacheItemPolicy _policy = new CacheItemPolicy() { SlidingExpiration = TimeSpan.FromMinutes(QuestionnaireContext.Current.Settings.Questionnaire.SlidingExpirationMinutes) };

			if(Contains(key)) {
				_cache.Set(key, obj, _policy);
			} else {
				_cache.Add(key, obj, _policy);
			}

			SaveUserCaches(key);
		}

		public void Save(int cacheMinutes, string key, object obj) {
			CacheItemPolicy _policy = new CacheItemPolicy() { SlidingExpiration = TimeSpan.FromMinutes(cacheMinutes) };

			if(Contains(key)) {
				_cache.Set(key, obj, _policy);
			} else {
				_cache.Add(key, obj, _policy);
			}
		}



		private void SaveUserCaches(string cacheKey) {
			if(QuestionnaireContext.Current.IsAuthenticated) {
				CacheItemPolicy _policy = new CacheItemPolicy() { SlidingExpiration = TimeSpan.FromMinutes(QuestionnaireContext.Current.Settings.Questionnaire.SlidingExpirationMinutes) };

				var key = string.Format("User_Caches_List_{0}", QuestionnaireContext.Current.UserId);
				var cacheList = new List<string> { };
				if(Contains(key)) {
					cacheList = Fetch(key) as List<string>;
				}

				if(!cacheList.Contains(cacheKey)) {
					cacheList.Add(cacheKey);
				}

				if(Contains(key)) {
					_cache.Set(key, cacheList, _policy);
				} else {
					_cache.Add(key, cacheList, _policy);
				}
			}
		}
	}
}
