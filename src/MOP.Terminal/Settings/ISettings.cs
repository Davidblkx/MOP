using System;
using System.Collections.Generic;

namespace MOP.Terminal.Settings
{
    public interface ISettings
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        Guid Id { get; }

        /// <summary>
        /// Gets the port to communicate, 0 to use random.
        /// </summary>
        /// <value>
        /// The port.
        /// </value>
        int Port { get; }

        /// <summary>
        /// Gets the address to use, localhost by default
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        string Hostname { get; }

        /// <summary>
        /// Gets a value indicating whether [log to file].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [log to file]; otherwise, <c>false</c>.
        /// </value>
        bool LogToFile { get; }

        string? DefaultHost { get; }

        /// <summary>
        /// Gets the hosts.
        /// </summary>
        /// <value>
        /// The hosts.
        /// </value>
        IEnumerable<IHostSettings> Hosts { get; }
    }
}
