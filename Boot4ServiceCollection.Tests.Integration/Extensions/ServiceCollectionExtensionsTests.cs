using Boot4ServiceCollection.Test.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Xunit;

namespace Boot4ServiceCollection.Tests.Integration.Extensions
{
    public sealed class ServiceCollectionExtensionsTests
    {
        private readonly ILogger _logger;
        private readonly IScopedTestClass _scopedTestClass;
        private readonly ISingletonTestClass _singletonTestClass;

        public ServiceCollectionExtensionsTests()
        {
            var services = new ServiceCollection();
            services.AddLogging();

            services.Boot();

            var serviceProvider = services.BuildServiceProvider();

            //configure NLog
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            loggerFactory.AddNLog();
            NLog.LogManager.LoadConfiguration("nlog.config");

            _logger = loggerFactory.CreateLogger<ServiceCollectionExtensionsTests>();
            _scopedTestClass = serviceProvider.GetService<IScopedTestClass>();
            _singletonTestClass = serviceProvider.GetService<ISingletonTestClass>();
        }

        [Fact]
        public void ScopedTestClassTest()
        {
            _logger.LogInformation("ScopedTestClassTest...");
            var result = _scopedTestClass.Method(10, 10);
            Assert.Equal(20, result);
        }

        [Fact]
        public void SingletonTestClassTest()
        {
            _logger.LogInformation("SingletonTestClassTest...");
            var result = _singletonTestClass.Method(10, 10);
            Assert.Equal(100, result);
        }
    }
}
