using System;

namespace MOP.Core.Plugins
{
    /// <summary>
    /// Mark class to be injected into the Host container
    /// </summary>
    /// <seealso cref="Attribute" />
    public class InjectableAttribute : Attribute
    {
        public string? Name { get; set; }
        public LifeCycle Life { get; set; } = LifeCycle.Transient;

        public InjectableAttribute() { }
    }
}
