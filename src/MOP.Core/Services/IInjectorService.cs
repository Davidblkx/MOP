using MOP.Core.Domain.Plugins;
using System;

namespace MOP.Core.Services
{
    /// <summary>
    /// IoC for MOP, allow to inject dependencies at runtime
    /// </summary>
    public interface IInjectorService
    {
        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetService<T>() where T : class;

        /// <summary>
        /// Gets the type implementation.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        object? GetService(Type type);

        /// <summary>
        /// Registers the service.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="lifeCycle">The life cycle.</param>
        /// <param name="name">The name.</param>
        void RegisterService<TService, TValue>(LifeCycle lifeCycle = LifeCycle.Transient)
            where TService : class
            where TValue : class, TService;

        /// <summary>
        /// Registers the service.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="lifeCycle">The life cycle.</param>
        void RegisterService<TService>(LifeCycle lifeCycle = LifeCycle.Transient)
            where TService : class;

        /// <summary>
        /// Registers the service.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="instanciator">The instance creator.</param>
        /// <param name="lifeCycle">The life cycle.</param>
        void RegisterService<TService>(Func<TService> instanceCreator, LifeCycle lifeCycle = LifeCycle.Transient) where TService : class;
        /// <summary>
        /// Registers the service.
        /// </summary>
        /// <param name="service">The service type.</param>
        /// <param name="lifeCycle">The life cycle.</param>
        /// <param name="name">The name.</param>
        void RegisterService(Type service, LifeCycle lifeCycle = LifeCycle.Transient);
        /// <summary>
        /// Registers the service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="lifeCycle">The life cycle.</param>
        /// <param name="name">The name.</param>
        void RegisterService(Type service, Type instance, LifeCycle lifeCycle = LifeCycle.Transient);
    }
}
