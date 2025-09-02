namespace P3k.FeaturesDependencyInjector.Bootstrap
{
   using P3k.FeaturesDependencyInjector.Container;
   using P3k.FeaturesDependencyInjector.Lifetime;
   using P3k.FeaturesDependencyInstaller;

   using System;
   using System.Linq;

   /// <summary>
   ///    Adapts <see cref="IFeatureServicesRegistrar" /> calls from feature installers
   ///    into service registrations on the underlying <see cref="IServiceCollection" />.
   /// </summary>
   public class FeatureServicesRegistrarAdapter : IFeatureServicesRegistrar
   {
      private readonly IServiceCollection _services;

      /// <summary>
      ///    Creates a new adapter around the specified <see cref="IServiceCollection" />.
      /// </summary>
      public FeatureServicesRegistrarAdapter(IServiceCollection services)
      {
         _services = services ?? throw new ArgumentNullException(nameof(services));
      }

      /// <summary>
      ///    Registers a singleton service where the service type is the same as the implementation type.
      ///    The same instance will be shared across the application lifetime.
      /// </summary>
      /// <param name="serviceType">The type to register as both service and implementation.</param>
      public void RegisterSingleton(Type serviceType)
      {
         _services.Add(new ServiceDescriptor(serviceType, serviceType, ServiceLifetime.Singleton, null));
      }

      /// <summary>
      ///    Registers a singleton mapping between a service and an implementation type.
      ///    The same instance will be reused for the lifetime of the application.
      /// </summary>
      public void RegisterSingleton<TService, TImplementation>()
         where TService : class where TImplementation : class, TService
      {
         _services.Add(
         new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Singleton, null));
      }

      /// <summary>
      ///    Registers a singleton mapping where the implementation type is the same as the service type.
      /// </summary>
      public void RegisterSingleton<TImplementation>()
         where TImplementation : class
      {
         _services.Add(
         new ServiceDescriptor(typeof(TImplementation), typeof(TImplementation), ServiceLifetime.Singleton, null));
      }

      /// <summary>
      ///    Registers a pre-created singleton instance of a service.
      /// </summary>
      public void RegisterSingleton<TService>(TService instance)
         where TService : class
      {
         _services.Add(ServiceDescriptor.SingletonInstance(instance));
      }

      /// <summary>
      ///    Registers a singleton service mapping using a factory delegate.
      /// </summary>
      public void RegisterSingleton<TService>(Func<IServiceProvider, TService> factory)
         where TService : class
      {
         _services.Add(ServiceDescriptor.Singleton(factory));
      }

      /// <summary>
      ///    Registers a transient service where the service type is the same as the implementation type.
      ///    A new instance will be created every time it is requested.
      /// </summary>
      /// <param name="serviceType">The type to register as both service and implementation.</param>
      public void RegisterTransient(Type serviceType)
      {
         _services.Add(new ServiceDescriptor(serviceType, serviceType, ServiceLifetime.Transient, null));
      }

      /// <summary>
      ///    Registers a transient mapping between a service and an implementation type.
      ///    A new instance will be created each time the service is resolved.
      /// </summary>
      public void RegisterTransient<TService, TImplementation>()
         where TService : class where TImplementation : class, TService
      {
         _services.Add(
         new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Transient, null));
      }

      /// <summary>
      ///    Registers a transient service mapping using a factory delegate.
      /// </summary>
      public void RegisterTransient<TService>(Func<IServiceProvider, TService> factory)
         where TService : class
      {
         _services.Add(new ServiceDescriptor(typeof(TService), null, ServiceLifetime.Transient, sp => factory(sp)));
      }
   }
}