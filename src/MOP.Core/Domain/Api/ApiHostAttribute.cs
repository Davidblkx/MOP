using System;

namespace MOP.Core.Domain.Api
{
    /// <summary>
    /// Allow class methods to be invoked by the ApiActor
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class)]
    public class ApiHostAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the description for this host.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the path to current actor.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the friendly name to use.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string? Name { get; set; }

        public ApiHostAttribute(string path)
        {
            Path = path;
        }
    }
}
