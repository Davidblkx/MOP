using MOP.Core.Services;
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

        IActorService? ActorService { get; }
        IConfigService? ConfigService { get; }
        IEventService? EventService { get; }
        ILogService? LogService { get; }
        IPluginService? PluginService { get; }

        void SetActorService(IActorService actorService, bool replace = false);
        void SetConfigService(IConfigService configService, bool replace = false);
        void SetEventService(IEventService eventService, bool replace = false);
        void SetLogService(IPluginService eventService, bool replace = false);
        void SetPluginService(IPluginService eventService, bool replace = false);
    }
}
