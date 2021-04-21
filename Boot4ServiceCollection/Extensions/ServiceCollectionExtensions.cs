using Boot4ServiceCollection.Attributes;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection Boot(this IServiceCollection serviceCollection) =>
            Boot(serviceCollection, AppDomain.CurrentDomain.GetAssemblies());

        public static IServiceCollection Boot(this IServiceCollection serviceCollection, IEnumerable<Assembly> assembliesToScan)
        {
            var attributedTypes = assembliesToScan
                .Where(a => !a.IsDynamic && a.GetName().Name != nameof(Boot4ServiceCollection) && a.GetName().Name != nameof(Boot4ServiceCollection.Attributes))
                .Distinct()
                .SelectMany(a => a.GetExportedTypes().Where(type => null != type.GetCustomAttribute<AddToServiceCollectionAttribute>()));

            foreach (var attributedType in attributedTypes)
            {
                AddAttributedType(serviceCollection, attributedType);
            }

            return serviceCollection;
        }
        
        private static void AddAttributedType(this IServiceCollection serviceCollection, Type attributedType)
        {
            var addAttribute = attributedType.GetCustomAttribute<AddToServiceCollectionAttribute>();
            if (null == addAttribute) return;
            serviceCollection.TryAdd(addAttribute, attributedType);
        }

        private static void TryAdd(this IServiceCollection serviceCollection, AddToServiceCollectionAttribute addAttribute, Type exportedType)
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
