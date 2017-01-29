using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Daemon {
	public class UnityResolver {
		private IUnityContainer _resolver;

		protected IUnityContainer Resolver {
			get {

				var _serviceLifetimeManagerType = typeof(TransientLifetimeManager);
				var _contextLifetimeManagerType = typeof(TransientLifetimeManager);

				if(_resolver == null) { 
					Unity.UnityConfig.Initialise(typeof(TransientLifetimeManager), typeof(TransientLifetimeManager), typeof(ContainerControlledLifetimeManager));
					var container = Questionnaire.Unity.UnityConfig.GetConfiguredContainer();

					_resolver = container;


				}

				return _resolver;
			}
		}

		public T Resolve<T>() {
			return Resolver.Resolve<T>();
		}
	}
}
