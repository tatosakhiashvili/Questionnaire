using System.Linq;
using System.Web.Mvc;
using Microsoft.Practices.Unity.Mvc;
using Microsoft.Practices.Unity;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Questionnaire.App_Start.UnityWebActivator), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(Questionnaire.App_Start.UnityWebActivator), "Shutdown")]

namespace Questionnaire.App_Start {
	/// <summary>Provides the bootstrapping for integrating Unity with ASP.NET MVC.</summary>
	public static class UnityWebActivator {
		/// <summary>Integrates Unity when the application starts.</summary>
		public static void Start() {
			Unity.UnityConfig.Initialise(typeof(TransientLifetimeManager), typeof(PerRequestLifetimeManager), typeof(ContainerControlledLifetimeManager));
			var container = Questionnaire.Unity.UnityConfig.GetConfiguredContainer();

			FilterProviders.Providers.Remove(FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().First());
			FilterProviders.Providers.Add(new UnityFilterAttributeFilterProvider(container));

			DependencyResolver.SetResolver(new UnityDependencyResolver(container));
			Microsoft.Web.Infrastructure.DynamicModuleHelper.DynamicModuleUtility.RegisterModule(typeof(UnityPerRequestHttpModule));

			// TODO: Uncomment if you want to use PerRequestLifetimeManager
			// Microsoft.Web.Infrastructure.DynamicModuleHelper.DynamicModuleUtility.RegisterModule(typeof(UnityPerRequestHttpModule));
		}

		/// <summary>Disposes the Unity container when the application is shut down.</summary>
		public static void Shutdown() {
			var container = UnityConfig.GetConfiguredContainer();
			container.Dispose();
		}
	}
}