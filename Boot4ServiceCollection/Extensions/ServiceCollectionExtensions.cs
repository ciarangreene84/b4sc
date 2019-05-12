using System;
using Boot4ServiceCollection.Attributes;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection Boot(this IServiceCollection serviceCollection, string path = null, string searchPattern = "*.dll")
        {
            if (string.IsNullOrEmpty(path))
            {
                path = Directory.GetCurrentDirectory();
            }

            foreach (var assemblyPath in Directory.GetFiles(path, searchPattern))
            {
                serviceCollection.AddFromAssembly(assemblyPath);
            }

            return serviceCollection;
        }

        private static void AddFromAssembly(this IServiceCollection serviceCollection, string assemblyPath)
        {
            var reflectedAssembly = Assembly.LoadFrom(assemblyPath);

            var enableBoot4ServiceCollection = reflectedAssembly.GetCustomAttribute<EnableBoot4ServiceCollectionAttribute>();
            if (enableBoot4ServiceCollection == null) return; 

            foreach (var exportedType in reflectedAssembly.GetExportedTypes())
            {
                var addAttribute = exportedType.GetCustomAttribute<AddToServiceCollectionAttribute>();
                serviceCollection.Add(addAttribute, exportedType);
            }
        }

        private static void Add(this IServiceCollection serviceCollection, AddToServiceCollectionAttribute addAttribute, Type exportedType)
        {
            switch (addAttribute)
            {
                case AddScopedAttribute _:
                    serviceCollection.TryAddScoped(addAttribute.InterfaceType, exportedType);
                    break;
                case AddSingletonAttribute _:
                    serviceCollection.TryAddSingleton(addAttribute.InterfaceType, exportedType);
                    break;
            }
        }
    }
}
