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
        private readonly ITransientTestClass _transientTestClass;
        private readonly INotAddedTestClass _notAddedTestClass;

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
            _transientTestClass = serviceProvider.GetService<ITransientTestClass>();
            _notAddedTestClass = serviceProvider.GetService<INotAddedTestClass>();
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

        [Fact]
        public void TransientTestClassTest()
        {
            _logger.LogInformation("TransientTestClassTest...");
            var result = _transientTestClass.Method(10, 10);
            Assert.Equal(1, result);
        }

        [Fact]
        public void NotAddedTestClassTest()
        {
            _logger.LogInformation("NotAddedTestClassTest...");
            Assert.Null(_notAddedTestClass);
        }

        [Fact]
        public void RequireEnableBoot4ServiceCollectionAttributeFalseTest()
        {
            _logger.LogInformation("RequireEnableBoot4ServiceCollectionAttributeFalseTest...");
            var services = new ServiceCollection();
            services.Boot(requireEnableBoot4ServiceCollectionAttribute: false);
        }

        [Fact]
        public void RequireEnableBoot4ServiceCollectionAttributeTrueTest()
        {
            _logger.LogInformation("RequireEnableBoot4ServiceCollectionAttributeTrueTest...");
            var services = new ServiceCollection();
            services.Boot(requireEnableBoot4ServiceCollectionAttribute: true);
        }
    }
}
