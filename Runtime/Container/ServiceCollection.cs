namespace P3k.FeaturesDependencyInjector.Container
{
   using P3k.FeaturesDependencyInjector.Lifetime;

   using System.Collections.Generic;
   using System.Linq;

   /// <summary>
   ///    Default implementation of <see cref="IServiceCollection" />.
   /// </summary>
   public class ServiceCollection : List<ServiceDescriptor>, IServiceCollection
   {
   }
}
