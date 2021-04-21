using Boot4ServiceCollection.Test.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using NLog.Extensions.Logging;
using Xunit;

namespace Boot4ServiceCollection.Tests.Integration.Extensions
{
    public sealed class ServiceCollectionExtensionsTests
    {
        private readonly ILogger _logger;

        public ServiceCollectionExtensionsTests()
        {
            var services = new ServiceCollection();
            //configure NLog            
            NLog.LogManager.LoadConfiguration("nlog.config");
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddNLog();
            });
            var serviceProvider = services.BuildServiceProvider();

            
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            _logger = loggerFactory.CreateLogger<ServiceCollectionExtensionsTests>();
        }

        [Fact]
        public void BootTest()
        {
            _logger.LogInformation("BootTest...");
            var services = new ServiceCollection().Boot();
            var serviceProvider = services.BuildServiceProvider();

            var scopedTestClass = serviceProvider.GetService<IScopedTestClass>();
            Assert.NotNull(scopedTestClass);
            var result1 = scopedTestClass.Method(10, 10);
            Assert.Equal(20, result1);

            var singletonTestClass = serviceProvider.GetService<ISingletonTestClass>();
            Assert.NotNull(singletonTestClass);
            var result2 = singletonTestClass.Method(10, 10);
            Assert.Equal(100, result2);

            var transientTestClass = serviceProvider.GetService<ITransientTestClass>();
            var result3 = transientTestClass.Method(10, 10);
            Assert.Equal(1, result3);

            var notAddedTestClass = serviceProvider.GetService<INotAddedTestClass>();
            Assert.Null(notAddedTestClass);
        }

        [Fact]
        public void BootTest_CurrentDomainAssemblies()
        {
            _logger.LogInformation("BootTest_CurrentDomainAssemblies...");
            var services = new ServiceCollection().Boot(AppDomain.CurrentDomain.GetAssemblies());
            var serviceProvider = services.BuildServiceProvider();

            var scopedTestClass = serviceProvider.GetService<IScopedTestClass>();
            Assert.NotNull(scopedTestClass);
            var result1 = scopedTestClass.Method(10, 10);
            Assert.Equal(20, result1);

            var singletonTestClass = serviceProvider.GetService<ISingletonTestClass>();
            Assert.NotNull(singletonTestClass);
            var result2 = singletonTestClass.Method(10, 10);
            Assert.Equal(100, result2);

            var transientTestClass = serviceProvider.GetService<ITransientTestClass>();
            var result3 = transientTestClass.Method(10, 10);
            Assert.Equal(1, result3);

            var notAddedTestClass = serviceProvider.GetService<INotAddedTestClass>();
            Assert.Null(notAddedTestClass);
        }
    }
}
