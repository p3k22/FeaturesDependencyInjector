namespace P3k.FeaturesDependencyInjector.Bootstrap
{
   using P3k.FeaturesDependencyInjector.Container;
   using P3k.FeaturesDependencyInjector.Extensions;
   using P3k.FeaturesDependencyInstaller;

   using System;
   using System.Linq;
   using System.Reflection;

   /// <summary>
   ///    Provides application bootstrap logic.
   ///    Discovers and installs all <see cref="IFeatureInstaller" /> implementations
   ///    found in loaded assemblies, and builds the <see cref="ServiceProvider" />.
   /// </summary>
   public static class Bootstrapper
   {
      /// <summary>
      ///    Initializes the DI container, discovering all feature installers automatically.
      /// </summary>
      /// <param name="namespacePrefix">
      ///    Optional namespace prefix to restrict which installers are loaded.
      ///    If null or empty, all installers are considered.
      /// </param>
      /// <param name="configure">
      ///    Optional callback to add extra service registrations before building the provider.
      /// </param>
      /// <returns>A fully built <see cref="ServiceProvider" />.</returns>
      public static ServiceProvider Initialize(
         string namespacePrefix = null,
         Action<IServiceCollection> configure = null)
      {
         var services = new ServiceCollection();
         var registrar = new FeatureServicesRegistrarAdapter(services);

         var assemblies = AppDomain.CurrentDomain.GetAssemblies();

         var installers = assemblies.SelectMany(a =>
               {
                  try
                  {
                     return a.GetTypes();
                  }
                  catch (ReflectionTypeLoadException ex)
                  {
                     return ex.Types.Where(t => t != null)!;
                  }
               }).Where(t =>
               t != null && typeof(IFeatureInstaller).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface
               && (string.IsNullOrEmpty(namespacePrefix) || t.Namespace?.StartsWith(namespacePrefix) == true))
            .Select(t => (IFeatureInstaller) Activator.CreateInstance(t)).ToList();

         foreach (var installer in installers)
         {
            installer.Install(registrar);
         }

         configure?.Invoke(services);

         return services.BuildServiceProvider();
      }
   }
}
