using System.Collections.Generic;

namespace MOP.Core.Domain.Api
{
    /// <summary>
    /// Allow to invoke some action from a remote program
    /// </summary>
    public class CommandInvoke
    {
        /// <summary>
        /// Gets or sets the actor path to invoke.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the method name to call.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the parameters to pass to method.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public Dictionary<int, object> Parameters { get; set; }

        public CommandInvoke()
        {
            Path = "";
            Name = "";
            Parameters = new Dictionary<int, object>();
        }
    }
}
