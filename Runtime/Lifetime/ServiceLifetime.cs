namespace P3k.FeaturesDependencyInjector.Lifetime
{
   using System.Linq;

   /// <summary>
   /// Represents the lifetime of a service registration.
   /// </summary>
   public enum ServiceLifetime
   {
      /// <summary>Single instance shared across the application.</summary>
      Singleton,

      /// <summary>New instance created each time the service is requested.</summary>
      Transient
   }
}
