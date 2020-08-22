using MOP.Core.Services;
using System;
using System.IO;

namespace MOP.Infra.Domain.Host
{
    /// <summary>
    /// Provides the core services
    /// </summary>
    public interface IHost
    {
        /// <summary>
        /// Gets the information of the current host
        /// </summary>
        /// <value>
        /// The information.
        /// </value>
        IHostInfo Info { get; }

        /// <summary>
        /// Gets the data directory.
        /// </summary>
        /// <value>
        /// The data directory.
        /// </value>
        DirectoryInfo DataDirectory { get; }

        /// <summary>
        /// Gets the temporary directory.
        /// </summary>
        /// <value>
        /// The temporary directory.
        /// </value>
        DirectoryInfo TempDirectory { get; }

        event EventHandler<int>? BeforeExit;
        event EventHandler<int>? Exit;

        /// <summary>
        /// Terminates with the specified exit code.
        /// </summary>
        /// <param name="exitCode">The exit code.</param>
        void Terminate(int exitCode);
    }
}
