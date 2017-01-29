using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Repositories;
using Questionnaire.Configuration;

namespace Questionnaire.MailSender {
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

				//if(_resolver == null) {
				//	_resolver = new UnityContainer();


				//	_resolver.RegisterType(typeof(Settings), Activator.CreateInstance(_serviceLifetimeManagerType) as LifetimeManager);
				//	_resolver.RegisterType(typeof(OracleContext), Activator.CreateInstance(_contextLifetimeManagerType) as LifetimeManager);

				//	var settings = _resolver.Resolve<Settings>();
				//	var isTestInstance = true; // settings.Questionnaire.IsTestInstance;

				//	var dummyRepositories = new List<Type> { };
				//	dummyRepositories.Add(typeof(Questionnaire.Repositories.DummyRepositories.UserRepository));
				//	dummyRepositories.Add(typeof(Questionnaire.Repositories.DummyRepositories.QuestionnaireRepository));
				//	dummyRepositories.Add(typeof(Questionnaire.Repositories.DummyRepositories.StoreRepository));
				//	dummyRepositories.Add(typeof(Questionnaire.Repositories.DummyRepositories.RoamingRepository));
				//	dummyRepositories.Add(typeof(Questionnaire.Repositories.DummyRepositories.InternationalPriceRepository));
				//	dummyRepositories.Add(typeof(Questionnaire.Repositories.DummyRepositories.InternationalCodeRepository));

				//	var typesToRegister = AllClasses.FromAssemblies(
				//									Assembly.Load("Questionnaire.Repositories"),
				//									Assembly.Load("Questionnaire.Services"))
				//							.Where(x => !dummyRepositories.Any(y => (isTestInstance ? y.FullName.Replace(".DummyRepositories", "") : y.FullName) == x.FullName));

				//	_resolver.RegisterTypes(
				//					typesToRegister,
				//					WithMappings.FromMatchingInterface,
				//					WithName.Default,
				//					x => Activator.CreateInstance(_serviceLifetimeManagerType) as LifetimeManager
				//	);
				//}
				return _resolver;
			}
		}

		public T Resolve<T>() {
			return Resolver.Resolve<T>();
		}
	}
}
