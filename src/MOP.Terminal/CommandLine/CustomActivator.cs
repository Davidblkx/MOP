using MOP.Terminal.Infra;
using System;
using System.CommandLine.Facilitator;

namespace MOP.Terminal.CommandLine
{
    /// <summary>
    /// Register and create instances of Command handlers
    /// </summary>
    /// <seealso cref="ICustomActivator" />
    class CustomActivator : ICustomActivator
    {
        public object? GetInstance(Type type)
        {
            return DependencyInjector.Container.GetInstance(type);
        }

        public void Register(Type type)
        {
            DependencyInjector.Container.RegisterSingleton(type, type);
        }
    }
}
