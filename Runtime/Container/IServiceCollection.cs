namespace P3k.FeaturesDependencyInjector.Container
{
   using P3k.FeaturesDependencyInjector.Lifetime;

   using System.Collections.Generic;
   using System.Linq;

   /// <summary>
   ///    Represents a collection of service registrations that will be used
   ///    to build a <see cref="ServiceProvider" />.
   /// </summary>
   public interface IServiceCollection : IList<ServiceDescriptor>
   {
   }
}
