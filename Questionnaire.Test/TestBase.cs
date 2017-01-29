using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using Questionnaire.Domain.Interfaces.Services;
using Questionnaire.Repositories;
using Questionnaire.Services;
using Questionnaire.Domain.Interfaces.Repositories;
using System.Reflection;

namespace Questionnaire.Test {
	[TestClass]
	public class TestBase {
		private IUnityContainer _resolver;

		protected IUnityContainer Resolver {
			get {
				if(_resolver == null) {
					_resolver = new UnityContainer();

					_resolver.RegisterTypes(
									 AllClasses.FromAssemblies(
													 Assembly.Load("Questionnaire.Repositories"),
													 Assembly.Load("Questionnaire.Services")
									 ),
									 WithMappings.FromMatchingInterface,
									 WithName.Default,
									 x => Activator.CreateInstance(typeof(TransientLifetimeManager)) as LifetimeManager
					 );

					_resolver.RegisterType(typeof(OracleContext), Activator.CreateInstance(typeof(TransientLifetimeManager)) as LifetimeManager);
				}
				return _resolver;
			}
		}

		protected T Resolve<T>() {
			return Resolver.Resolve<T>();
		}
	}
}
