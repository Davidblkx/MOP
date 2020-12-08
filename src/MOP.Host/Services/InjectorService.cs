using MOP.Core.Domain.Plugins;
using MOP.Core.Services;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MOP.Host.Test")]
namespace MOP.Host.Services
{
    internal class InjectorService : IInjectorService
    {
        private static bool INIT = false;
        private Dictionary<Type, (Type, Lifestyle)> _pending;
        private Dictionary<Type, (Func<object>, Lifestyle)> _pendingInstance;

        public Container Container { get; }

        public InjectorService(bool forceOneInstance = true)
        {
            if (forceOneInstance && INIT) throw new AccessViolationException("Service already instantiated");
            Container = new Container();
            Container.Options.AllowOverridingRegistrations = true;

            _pending = new Dictionary<Type, (Type, Lifestyle)>();
            _pendingInstance = new Dictionary<Type, (Func<object>, Lifestyle)>();

            Container.ResolveUnregisteredType += ResolveUnregisterTypes;

            INIT = true;
        }

        public T GetService<T>() where T : class
            => Container.GetInstance<T>();

        public object? GetService(Type type)
            => Container.GetInstance(type);

        public void RegisterService(Type service, LifeCycle lifeCycle = LifeCycle.Transient)
            => SafeRegisterService(service, service, lifeCycle);

        public void RegisterService(Type service, Type instance, LifeCycle lifeCycle = LifeCycle.Transient)
            => SafeRegisterService(service, instance, lifeCycle);

        public void RegisterService<TService, TValue>(LifeCycle lifeCycle = LifeCycle.Transient)
            where TService : class
            where TValue : class, TService
            => SafeRegisterService(typeof(TService), typeof(TValue), lifeCycle);

        public void RegisterService<TService>(LifeCycle lifeCycle = LifeCycle.Transient)
            where TService : class
            => SafeRegisterService(typeof(TService), typeof(TService), lifeCycle);

        public void RegisterService<TService>(Func<TService> instanceCreator, LifeCycle lifeCycle = LifeCycle.Transient)
            where TService : class
            => SafeRegisterService(instanceCreator, lifeCycle);

        private void SafeRegisterService(Type service, Type instance, LifeCycle lifeCycle)
        {
            if (!Container.IsLocked)
            {
                Container.Register(service, instance, GetLifestyle(lifeCycle));
            } else {
                _pending.Add(service, (instance, GetLifestyle(lifeCycle)));
            }
        }

        private void SafeRegisterService<TService>(Func<TService> instanceCreator, LifeCycle lifeCycle)
            where TService : class
        {
            if (!Container.IsLocked)
            {
                Container.Register(instanceCreator, GetLifestyle(lifeCycle));
            } else {
                _pendingInstance.Add(typeof(TService), (instanceCreator, GetLifestyle(lifeCycle)));
            }
        }

        private void ResolveUnregisterTypes(object? sender, UnregisteredTypeEventArgs e)
        {
            var type = e.UnregisteredServiceType;
            if (_pending.ContainsKey(type))
            {
                var v = _pending[type];
                e.Register(v.Item2.CreateRegistration(v.Item1, Container));
            }
            if (_pendingInstance.ContainsKey(type))
            {
                var v = _pendingInstance[type];
                e.Register(v.Item2.CreateRegistration(type, v.Item1, Container));
            }
        }

        public static void ResetInitStatus()
        {
            INIT = false;
        }

        private Lifestyle GetLifestyle(LifeCycle lifeCycle)
            => lifeCycle == LifeCycle.Singleton ?
                Lifestyle.Singleton : Lifestyle.Transient;
    }
}
