using System;

namespace MOP.Core.Domain.Api
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ApiActionAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the description of current action.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string? Description { get; set; }
        /// <summary>
        /// Gets or sets the description for return information.
        /// </summary>
        /// <value>
        /// The returns.
        /// </value>
        public string? Returns { get; set; }

        public ApiActionAttribute() { }
    }
}
