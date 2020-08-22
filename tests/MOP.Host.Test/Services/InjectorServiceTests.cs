using MOP.Core.Domain.Plugins;
using MOP.Host.Services;
using System;
using Xunit;

namespace MOP.Host.Test.Services
{
    public class InjectorServiceTests
    {
        [Fact]
        public void TestInjector_RegisterType_TypeInstance()
        {
            InjectorService.ResetInitStatus();
            var injector = new InjectorService();

            injector.RegisterService(typeof(ITestInterface), typeof(TestImplConst));
            var service = injector.GetService<ITestInterface>();

            Assert.NotNull(service);
            Assert.Equal(new TestImplConst().Name, service.Name);
        }

        [Fact]
        public void TestInjector_RegisterType_Instance()
        {
            InjectorService.ResetInitStatus();
            var injector = new InjectorService();

            injector.RegisterService(typeof(TestImplConst));
            var service = injector.GetService<TestImplConst>();

            Assert.NotNull(service);
            Assert.Equal(new TestImplConst().Name, service.Name);
        }

        [Fact]
        public void TestInjector_RegisterGenericType_TypeInstance()
        {
            InjectorService.ResetInitStatus();
            var injector = new InjectorService();

            injector.RegisterService<ITestInterface, TestImplConst>();
            var service = injector.GetService<ITestInterface>();

            Assert.NotNull(service);
            Assert.Equal(new TestImplConst().Name, service.Name);
        }

        [Fact]
        public void TestInjector_RegisterGenericType_InstanceCreator()
        {
            InjectorService.ResetInitStatus();
            var injector = new InjectorService();
            var target = "PIKA";

            injector.RegisterService<ITestInterface>(() => new TestImpl(target));
            var service = injector.GetService<ITestInterface>();

            Assert.NotNull(service);
            Assert.Equal(target, service.Name);
        }

        [Fact]
        public void TestInjector_InitConstrait()
        {
            InjectorService.ResetInitStatus();
            var injector = new InjectorService();

            Assert.NotNull(injector);
            Assert.Throws<AccessViolationException>(() => new InjectorService());
        }

        [Fact]
        public void TestInjector_singleton()
        {
            InjectorService.ResetInitStatus();
            var injector = new InjectorService();
            var target = "PIKA";
            var target2 = "EKANS";

            injector.RegisterService<ITestInterface>(() => new TestImpl(target), LifeCycle.Singleton);
            var service = injector.GetService<ITestInterface>();

            Assert.NotNull(service);
            Assert.Equal(target, service.Name);

            service.Name = target2;
            var newService = injector.GetService<ITestInterface>();
            Assert.Equal(target2, newService.Name);
        }

        [Fact]
        public void TestInjector_transient_default()
        {
            InjectorService.ResetInitStatus();
            var injector = new InjectorService();
            var target = "PIKA";
            var target2 = "EKANS";

            injector.RegisterService<ITestInterface>(() => new TestImpl(target));
            var service = injector.GetService<ITestInterface>();

            Assert.NotNull(service);
            Assert.Equal(target, service.Name);

            service.Name = target2;
            var newService = injector.GetService<ITestInterface>();
            Assert.Equal(target, newService.Name);
        }
    }

    internal interface ITestInterface
    {
        string Name { get; set; }
    }

    internal class TestImplConst : ITestInterface
    {
        public string Name { get; set; } = "BATATAS";
    }

    internal class TestImpl : ITestInterface
    {
        public string Name { get; set; }
        public TestImpl(string name) { Name = name; }
    }
}
