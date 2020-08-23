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
        private readonly Container _container;

        public InjectorService(bool forceOneInstance = true)
        {
            if (forceOneInstance && INIT) throw new AccessViolationException("Service already instantiated");
            _container = new Container();
            _container.Options.AllowOverridingRegistrations = true;
            INIT = true;
        }

        public T GetService<T>() where T : class
            => _container.GetInstance<T>();

        public void RegisterService(Type service, LifeCycle lifeCycle = LifeCycle.Transient)
            => _container.Register(service, service, GetLifestyle(lifeCycle));

        public void RegisterService(Type service, Type instance, LifeCycle lifeCycle = LifeCycle.Transient)
            => _container.Register(service, instance, GetLifestyle(lifeCycle));

        public void RegisterService<TService, TValue>(LifeCycle lifeCycle = LifeCycle.Transient)
            where TService : class
            where TValue : class, TService
            => _container.Register<TService, TValue>(GetLifestyle(lifeCycle));

        public void RegisterService<TService>(Func<TService> instanceCreator, LifeCycle lifeCycle = LifeCycle.Transient)
            where TService : class
            => _container.Register(instanceCreator, GetLifestyle(lifeCycle));

        public static void ResetInitStatus()
        {
            INIT = false;
        }

        private Lifestyle GetLifestyle(LifeCycle lifeCycle)
            => lifeCycle == LifeCycle.Singleton ?
                Lifestyle.Singleton : Lifestyle.Transient;
    }
}
