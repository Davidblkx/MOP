using MOP.Core.Domain.Plugins;
using MOP.Core.Services;
using SimpleInjector;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MOP.Host.Test")]
namespace MOP.Host.Services
{
    internal class InjectorService : IInjectorService
    {
        private static bool INIT = false;

        public Container Container { get; }

        public InjectorService(bool forceOneInstance = true)
        {
            if (forceOneInstance && INIT) throw new AccessViolationException("Service already instantiated");
            Container = new Container();
            Container.Options.AllowOverridingRegistrations = true;
            INIT = true;
        }

        public T GetService<T>() where T : class
            => Container.GetInstance<T>();

        public object? GetService(Type type)
            => Container.GetInstance(type);

        public void RegisterService(Type service, LifeCycle lifeCycle = LifeCycle.Transient)
            => Container.Register(service, service, GetLifestyle(lifeCycle));

        public void RegisterService(Type service, Type instance, LifeCycle lifeCycle = LifeCycle.Transient)
            => Container.Register(service, instance, GetLifestyle(lifeCycle));

        public void RegisterService<TService, TValue>(LifeCycle lifeCycle = LifeCycle.Transient)
            where TService : class
            where TValue : class, TService
            => Container.Register<TService, TValue>(GetLifestyle(lifeCycle));

        public void RegisterService<TService>(Func<TService> instanceCreator, LifeCycle lifeCycle = LifeCycle.Transient)
            where TService : class
            => Container.Register(instanceCreator, GetLifestyle(lifeCycle));

        public static void ResetInitStatus()
        {
            INIT = false;
        }

        private Lifestyle GetLifestyle(LifeCycle lifeCycle)
            => lifeCycle == LifeCycle.Singleton ?
                Lifestyle.Singleton : Lifestyle.Transient;
    }
}
