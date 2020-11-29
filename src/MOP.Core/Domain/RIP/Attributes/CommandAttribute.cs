using System;

namespace MOP.Core.Domain.RIP.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandAttribute : Attribute
    {
        public string Name { get; set; }

        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the target service type to search for in container.
        /// </summary>
        /// <value>
        /// The target.
        /// </value>
        public Type? Target { get; set; }

        public CommandAttribute(string name) { Name = name; }
    }
}
