using System;

namespace MOP.Core.Domain.Api
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ActorInstanceAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the path to invoke this command actions.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        public string Path { get; set; }
        public string? Description { get; set; }

        public ActorInstanceAttribute(string path, string? description = default)
        {
            Path = path;
            Description = description;
        }
    }
}
