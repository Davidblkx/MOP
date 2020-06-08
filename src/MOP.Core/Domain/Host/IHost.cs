using MOP.Core.Services;
using System;
using System.IO;

namespace MOP.Core.Domain.Host
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

        IActorService? ActorService { get; }
        IConfigService? ConfigService { get; }
        IEventService? EventService { get; }
        ILogService? LogService { get; }
        IPluginService? PluginService { get; }

        void SetActorService(IActorService actorService, bool replace = false);
        void SetConfigService(IConfigService configService, bool replace = false);
        void SetEventService(IEventService eventService, bool replace = false);
        void SetLogService(ILogService logService, bool replace = false);
        void SetPluginService(IPluginService pluginService, bool replace = false);

        /// <summary>
        /// Terminates with the specified exit code.
        /// </summary>
        /// <param name="exitCode">The exit code.</param>
        void Terminate(int exitCode);
    }
}
