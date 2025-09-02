namespace P3k.FeaturesDependencyInjector.Container
{
   using P3k.FeaturesDependencyInjector.Lifetime;

   using System;
   using System.Collections.Concurrent;
   using System.Collections.Generic;
   using System.Linq;
   using System.Reflection;

   /// <summary>
   ///    Default dependency injection service provider.
   ///    Resolves services based on registered descriptors.
   /// </summary>
   public class ServiceProvider : IServiceProvider, IDisposable
   {
      private readonly ConcurrentDictionary<ServiceDescriptor, object> _singletons =
         new ConcurrentDictionary<ServiceDescriptor, object>();

      private readonly IReadOnlyList<ServiceDescriptor> _descriptors;

      internal ServiceProvider(IEnumerable<ServiceDescriptor> descriptors)
      {
         _descriptors = descriptors.ToList();
      }

      /// <summary>
      ///    Disposes all tracked singleton instances that implement <see cref="IDisposable" />.
      /// </summary>
      public void Dispose()
      {
         foreach (var obj in _singletons.Values.OfType<IDisposable>())
         {
            obj.Dispose();
         }

         _singletons.Clear();
      }

      /// <inheritdoc />
      public object GetService(Type serviceType)
      {
         // Handle IEnumerable<T>
         if (serviceType.IsGenericType && serviceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
         {
            var itemType = serviceType.GetGenericArguments()[0];
            var matches = _descriptors.Where(d => d.ServiceType == itemType).ToList();
            var instances = matches.Select(ResolveDescriptor).ToArray();

            var array = Array.CreateInstance(itemType, instances.Length);
            instances.CopyTo(array, 0);
            return array;
         }

         var descriptor = _descriptors.FirstOrDefault(d => d.ServiceType == serviceType);
         if (descriptor == null)
         {
            throw new InvalidOperationException($"Service {serviceType.FullName} not registered.");
         }

         return ResolveDescriptor(descriptor);
      }

      /// <summary>
      ///    Resolves and returns a service of type <typeparamref name="T" />.
      /// </summary>
      public T GetService<T>()
      {
         return (T) GetService(typeof(T));
      }

      /// <summary>
      ///    Resolves and returns all registered services of type <typeparamref name="T" />.
      /// </summary>
      /// <typeparam name="T">The service type to resolve.</typeparam>
      /// <returns>An enumerable of all registered instances of the specified type.</returns>
      public IEnumerable<T> GetServicesOfType<T>()
      {
         var matches = _descriptors.Where(d => d.ServiceType == typeof(T)).ToList();
         return matches.Select(ResolveDescriptor).Cast<T>();
      }

      private object CreateInstance(ServiceDescriptor desc)
      {
         if (desc.ImplementationFactory != null)
         {
            return desc.ImplementationFactory(this);
         }

         var implType = desc.ImplementationType
                        ?? throw new InvalidOperationException(
                        $"No implementation type for {desc.ServiceType.FullName}");

         var ctor = implType.GetConstructors(BindingFlags.Public | BindingFlags.Instance).FirstOrDefault();

         if (ctor == null)
         {
            throw new InvalidOperationException($"No suitable constructor found for {implType.FullName}");
         }

         var args = ctor.GetParameters().Select(p => GetService(p.ParameterType)).ToArray();
         return Activator.CreateInstance(implType, args);
      }

      private object ResolveDescriptor(ServiceDescriptor desc)
      {
         if (desc.Lifetime == ServiceLifetime.Singleton)
         {
            if (!_singletons.TryGetValue(desc, out var singleton))
            {
               singleton = CreateInstance(desc);
               _singletons[desc] = singleton;
            }

            return singleton;
         }

         return CreateInstance(desc);
      }
   }
}