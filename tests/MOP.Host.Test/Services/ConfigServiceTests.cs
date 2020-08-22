using MOP.Infra.Domain.Host;
using MOP.Host.Services;
using MOP.Host.Test.Mocks;
using MOP.Infra.Services;
using Optional.Unsafe;
using System;
using System.Threading.Tasks;
using Xunit;

using static MOP.Host.Test.Mocks.MockBuilder;

namespace MOP.Host.Test.Services
{
    [Collection("HOST")]
    public class ConfigServiceTests
    {
        public IHost Host { get; }

        public ConfigServiceTests(MockBuilder mock)
        {
            Host = mock.Host;
        }

        private ConfigService CreateService()
        {
            return new ConfigService(Host, injector.GetService<ILogService>());
        }

        [Fact]
        public async Task ReloadConfigObject_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateService();

            // Act
            var reload1 = await service.ReloadConfigObject(false);
            var reload2 = await service.ReloadConfigObject(false);
            var reload3 = await service.ReloadConfigObject(true);

            // Assert
            Assert.True(reload1);
            Assert.True(reload2);
            Assert.True(reload3);
        }

        [Fact]
        public async Task LoadConfig_StateUnderTest_LoadDefault()
        {
            // Arrange
            var service = CreateService();
            await service.ReloadConfigObject();
            var id = Guid.NewGuid();
            var target = new TargetSubject { Name = "default", Age = 10 };

            // Act
            var result = (await service.LoadConfig(id, target)).ValueOrDefault();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(target.Name, result.Name);
            Assert.Equal(target.Age, result.Age);
        }

        [Fact]
        public async Task LoadConfig_StateUnderTest_CantFindId()
        {
            // Arrange
            var service = CreateService();
            await service.ReloadConfigObject();
            var id = Guid.NewGuid();

            // Act
            var result = await service.LoadConfig<TargetSubject>(id);

            // Assert
            Assert.False(result.HasValue);
        }

        [Fact]
        public async Task SaveConfig_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateService();
            await service.ReloadConfigObject();
            var id = Guid.NewGuid();
            var targetDefault = new TargetSubject { Name = "default", Age = 10 };
            var targetValue = new TargetSubject { Name = "Value", Age = 24 };

            // Act
            var saveRes = await service.SaveConfig(id, targetValue);
            var loadRes = (await service.LoadConfig(id, targetDefault)).ValueOrDefault();

            // Assert
            Assert.True(saveRes);
            Assert.NotNull(loadRes);
            Assert.Equal(targetValue.Name, loadRes.Name);
            Assert.Equal(targetValue.Age, loadRes.Age);
        }

        private class TargetSubject
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }
    }
}
