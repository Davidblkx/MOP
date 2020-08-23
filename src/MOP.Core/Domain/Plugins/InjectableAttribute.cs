using System;

namespace MOP.Core.Domain.Plugins
{
    /// <summary>
    /// Mark class to be injected into the Host container
    /// </summary>
    /// <seealso cref="Attribute" />
    [AttributeUsage(AttributeTargets.Class)]
    public class InjectableAttribute : Attribute
    {
        public LifeCycle Life { get; set; }

        public InjectableAttribute(LifeCycle lifeCycle = LifeCycle.Transient)
        {
            Life = lifeCycle;
        }
    }
}
