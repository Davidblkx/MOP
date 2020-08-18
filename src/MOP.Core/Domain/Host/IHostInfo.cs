
using Semver;
using System;

namespace MOP.Infra.Domain.Host
{
    /// <summary>
    /// Information of a MOP host
    /// </summary>
    public interface IHostInfo
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        Guid Id { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; }

        /// <summary>
        /// Gets the MOP core version.
        /// </summary>
        /// <value>
        /// The core version.
        /// </value>
        SemVersion CoreVersion { get; }
    }
}
