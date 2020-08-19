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
        Guid Id { get; set; }

        /// <summary>
        /// Gets the port to communicate, 0 to use random.
        /// </summary>
        /// <value>
        /// The port.
        /// </value>
        int Port { get; set; }

        /// <summary>
        /// Gets the address to use, localhost by default
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        string Hostname { get; set; }

        /// <summary>
        /// Gets a value indicating whether [log to file].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [log to file]; otherwise, <c>false</c>.
        /// </value>
        bool LogToFile { get; set; }

        /// <summary>
        /// Gets or sets the log level.
        /// 0 - Disabled
        /// 1 - Fatal
        /// 2 - Error
        /// 3 - Warning
        /// 4 - Information
        /// 5 - Verbose
        /// </summary>
        /// <value>
        /// The log level.
        /// </value>
        int LogLevel { get; set; }

        string? DefaultHost { get; set; }

        /// <summary>
        /// Gets the hosts.
        /// </summary>
        /// <value>
        /// The hosts.
        /// </value>
        List<HostSettings> Hosts { get; set; }
    }
}
