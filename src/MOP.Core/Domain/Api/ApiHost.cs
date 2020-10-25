using System.Collections.Generic;

namespace MOP.Core.Domain.Api
{
    /// <summary>
    /// Represents and actor that contains actions to be invoked by APIAction
    /// </summary>
    public class ApiHost
    {
        /// <summary>
        /// Gets or sets the path to actor
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the friendly name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        public string Description { get; set; } = "No description";

        public IEnumerable<ApiAction> Actions { get; set; }
            = new List<ApiAction>();

        public ApiHost(string path, string? name = default)
        {
            Path = path;
            Name = name ?? path;
        }
    }
}
