namespace P3k.FeaturesDependencyInjector.Lifetime
{
   using System;
   using System.Linq;

   /// <summary>
   /// Describes a single service registration, including its contract,
   /// implementation, lifetime, and optional factory delegate.
   /// </summary>
   public class ServiceDescriptor
   {
      public ServiceDescriptor(
         Type serviceType,
         Type implementationType,
         ServiceLifetime lifetime,
         Func<IServiceProvider, object> implementationFactory)
      {
         ServiceType = serviceType;
         ImplementationType = implementationType;
         Lifetime = lifetime;
         ImplementationFactory = implementationFactory;
      }

      /// <summary>The service type being registered.</summary>
      public Type ServiceType { get; }

      /// <summary>The concrete implementation type.</summary>
      public Type ImplementationType { get; }

      /// <summary>The service lifetime.</summary>
      public ServiceLifetime Lifetime { get; }

      /// <summary>Factory delegate for constructing the service instance.</summary>
      public Func<IServiceProvider, object> ImplementationFactory { get; }

      /// <summary>Create a singleton registration for TService → TImplementation.</summary>
      public static ServiceDescriptor Singleton<TService, TImplementation>()
         where TImplementation : TService =>
         new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Singleton, null);

      /// <summary>Create a singleton registration using a factory.</summary>
      public static ServiceDescriptor Singleton<TService>(Func<IServiceProvider, TService> factory) =>
         new ServiceDescriptor(typeof(TService), null, ServiceLifetime.Singleton, sp => factory(sp));

      /// <summary>Create a singleton registration for a pre-existing instance.</summary>
      public static ServiceDescriptor SingletonInstance<TService>(TService instance) =>
         new ServiceDescriptor(typeof(TService), instance.GetType(), ServiceLifetime.Singleton, _ => instance);

      /// <summary>Create a transient registration for TService → TImplementation.</summary>
      public static ServiceDescriptor Transient<TService, TImplementation>()
         where TImplementation : TService =>
         new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Transient, null);

      /// <summary>Create a transient registration using a factory.</summary>
      public static ServiceDescriptor Transient<TService>(Func<IServiceProvider, TService> factory) =>
         new ServiceDescriptor(typeof(TService), null, ServiceLifetime.Transient, sp => factory(sp));
   }
}
