namespace P3k.FeaturesDependencyInjector.Extensions
{
   using P3k.FeaturesDependencyInjector.Container;
   using P3k.FeaturesDependencyInjector.Lifetime;

   using System;
   using System.Linq;

   /// <summary>
   ///    Extension methods for registering services in an <see cref="IServiceCollection" />.
   /// </summary>
   public static class ServiceCollectionExtensions
   {
      /// <summary>
      ///    Registers a singleton service where the service type is the same as the implementation type.
      ///    The same instance will be reused for the entire application lifetime.
      /// </summary>
      /// <param name="services">The service collection to add the registration to.</param>
      /// <param name="serviceType">The type to register as both service and implementation.</param>
      /// <returns>The service collection for method chaining.</returns>
      public static IServiceCollection AddSingleton(this IServiceCollection services, Type serviceType)
      {
         services.Add(new ServiceDescriptor(serviceType, serviceType, ServiceLifetime.Singleton, null));
         return services;
      }

      /// <summary>
      ///    Registers a singleton mapping between a service type and an implementation type.
      ///    The same instance will be reused for the entire application lifetime.
      /// </summary>
      public static IServiceCollection AddSingleton<TService, TImpl>(this IServiceCollection services)
         where TImpl : TService
      {
         services.Add(ServiceDescriptor.Singleton<TService, TImpl>());
         return services;
      }

      /// <summary>
      ///    Registers a pre-created singleton instance.
      ///    That instance will always be returned when resolving the service.
      /// </summary>
      public static IServiceCollection AddSingleton<TService>(this IServiceCollection services, TService instance)
      {
         services.Add(ServiceDescriptor.SingletonInstance(instance));
         return services;
      }

      /// <summary>
      ///    Registers a singleton service using a factory delegate.
      ///    The factory is invoked once and the same instance is returned thereafter.
      /// </summary>
      public static IServiceCollection AddSingleton<TService>(
         this IServiceCollection services,
         Func<IServiceProvider, TService> factory)
      {
         services.Add(ServiceDescriptor.Singleton(factory));
         return services;
      }

      /// <summary>
      ///    Registers a transient service where the service type is the same as the implementation type.
      ///    A new instance is created every time the service is requested.
      /// </summary>
      /// <param name="services">The service collection to add the registration to.</param>
      /// <param name="serviceType">The type to register as both service and implementation.</param>
      /// <returns>The service collection for method chaining.</returns>
      public static IServiceCollection AddTransient(this IServiceCollection services, Type serviceType)
      {
         services.Add(new ServiceDescriptor(serviceType, serviceType, ServiceLifetime.Transient, null));
         return services;
      }

      /// <summary>
      ///    Registers a transient mapping between a service type and an implementation type.
      ///    A new instance is created every time the service is requested.
      /// </summary>
      public static IServiceCollection AddTransient<TService, TImpl>(this IServiceCollection services)
         where TImpl : TService
      {
         services.Add(ServiceDescriptor.Transient<TService, TImpl>());
         return services;
      }

      /// <summary>
      ///    Registers a transient service using a factory delegate.
      ///    The factory is invoked every time the service is resolved.
      /// </summary>
      public static IServiceCollection AddTransient<TService>(
         this IServiceCollection services,
         Func<IServiceProvider, TService> factory)
      {
         services.Add(ServiceDescriptor.Transient(factory));
         return services;
      }

      /// <summary>
      ///    Builds a new <see cref="ServiceProvider" /> from the registered services.
      /// </summary>
      public static ServiceProvider BuildServiceProvider(this IServiceCollection services)
      {
         return new ServiceProvider(services);
      }
   }
}