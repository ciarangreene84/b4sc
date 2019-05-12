using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using Xunit;

namespace Boot4ServiceCollection.Tests.Integration
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve +=
                (s, e) => Assembly.ReflectionOnlyLoad(e.Name);

            var services = new ServiceCollection();
            services.AddOptions();
            services.AddLogging();


            services.Boot();

            var serviceProvider = services.BuildServiceProvider();
            //var service = serviceProvider.GetService<IAgentsDbContext>();
        }
    }
}
