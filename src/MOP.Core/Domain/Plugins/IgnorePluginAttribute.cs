using System;

namespace MOP.Core.Domain.Plugins
{
    /// <summary>
    /// Class with this attribute are ignored during plugin load time
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class)]
    public class IgnorePluginAttribute : Attribute { }
}
