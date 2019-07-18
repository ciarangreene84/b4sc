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
        /// <summary>
        /// Boots attribute-based injection classes.
        /// By default, loops through all the *.dll files in the current directory. (Path and search pattern can be altered)
        /// By default, each assembly must have the EnableBoot4ServiceCollectionAttribute. This can be disabled.
        /// </summary>
        /// <param name="serviceCollection">Extension target</param>
        /// <param name="path">Path containing the assemblies to process. Defaults to Directory.GetCurrentDirectory()</param>
        /// <param name="searchPattern">Search pattern for assemblies to process. Defaults to "*.dll"</param>
        /// <param name="requireEnableBoot4ServiceCollectionAttribute">Determines whether to check for the EnableBoot4ServiceCollectionAttribute in each assembly. Defaults to true</param>
        /// <returns>Extension target</returns>
        public static IServiceCollection Boot(this IServiceCollection serviceCollection, string path = null, string searchPattern = "*.dll", bool requireEnableBoot4ServiceCollectionAttribute = true)
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

        private static void AddFromAssembly(this IServiceCollection serviceCollection, string assemblyPath, bool requireEnableBoot4ServiceCollectionAttribute = true)
        {
            var reflectedAssembly = Assembly.LoadFrom(assemblyPath);

            var enableBoot4ServiceCollection = reflectedAssembly.GetCustomAttribute<EnableBoot4ServiceCollectionAttribute>();
            if (requireEnableBoot4ServiceCollectionAttribute && enableBoot4ServiceCollection == null) return; 

            foreach (var exportedType in reflectedAssembly.GetExportedTypes())
            {
                var addAttribute = exportedType.GetCustomAttribute<AddToServiceCollectionAttribute>();
                if (null == addAttribute) continue;
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
                case AddTransientAttribute _:
                    serviceCollection.TryAddTransient(addAttribute.InterfaceType, exportedType);
                    break;
            }
        }
    }
}
