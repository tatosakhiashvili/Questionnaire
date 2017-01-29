using Microsoft.Practices.Unity;
using Questionnaire.Configuration;
using Questionnaire.Domain.Interfaces.Services;
using Questionnaire.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Unity {
	public class UnityConfig : IServiceProvider {
		private static Type _serviceLifetimeManagerType;
		private static Type _contextLifetimeManagerType;
		private static Type _providerLifetimeManagerType;

		private static Lazy<IUnityContainer> _container = new Lazy<IUnityContainer>(() => {
			var container = new UnityContainer();
			RegisterTypes(container);
			return container;
		});

		public static void Initialise(Type serviceLifetimeManagerType, Type contextLifetimeManagerType, Type providerLifetimeManagerType) {
			_serviceLifetimeManagerType = serviceLifetimeManagerType;
			_contextLifetimeManagerType = contextLifetimeManagerType;
			_providerLifetimeManagerType = providerLifetimeManagerType;
		}

		private static void RegisterTypes(IUnityContainer container) {
			container.RegisterType(typeof(Settings), Activator.CreateInstance(_serviceLifetimeManagerType) as LifetimeManager);
			container.RegisterType(typeof(OracleContext), Activator.CreateInstance(_contextLifetimeManagerType) as LifetimeManager);

			var settings = container.Resolve<Settings>();
			var isTestInstance = settings.Questionnaire.IsTestInstance;

			var dummyRepositories = new List<Type> { };
			dummyRepositories.Add(typeof(Questionnaire.Repositories.DummyRepositories.UserRepository));
			dummyRepositories.Add(typeof(Questionnaire.Repositories.DummyRepositories.QuestionnaireRepository));
			dummyRepositories.Add(typeof(Questionnaire.Repositories.DummyRepositories.StoreRepository));
			dummyRepositories.Add(typeof(Questionnaire.Repositories.DummyRepositories.RoamingRepository));
			dummyRepositories.Add(typeof(Questionnaire.Repositories.DummyRepositories.InternationalPriceRepository));
			dummyRepositories.Add(typeof(Questionnaire.Repositories.DummyRepositories.InternationalCodeRepository));
			dummyRepositories.Add(typeof(Questionnaire.Repositories.DummyRepositories.ChatRepository));

			var typesToRegister = AllClasses.FromAssemblies(
											Assembly.Load("Questionnaire.Repositories"),
											Assembly.Load("Questionnaire.Services"))
									.Where(x => !dummyRepositories.Any(y => (isTestInstance ? y.FullName.Replace(".DummyRepositories", "") : y.FullName) == x.FullName));

			container.RegisterTypes(
							typesToRegister,
							WithMappings.FromMatchingInterface,
							WithName.Default,
							x => Activator.CreateInstance(_serviceLifetimeManagerType) as LifetimeManager
			);
		}

		public static IUnityContainer GetConfiguredContainer() {
			return _container.Value;
		}

		public object GetService(Type serviceType) {
			return _container.Value.Resolve(serviceType);
		}
	}
}
