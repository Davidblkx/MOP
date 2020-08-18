using MOP.Infra.Domain.Host;
using MOP.Infra.Services;
using MOP.Host.Domain;
using MOP.Host.Services;
using Serilog;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using static MOP.Infra.Tools.InfoBuilder;

[assembly: InternalsVisibleTo("MOP.Host.Test")]
namespace MOP.Host
{
    internal class MopHost : IHost
    {
        public IHostInfo Info { get; }
        /// <summary>
        /// Gets the data directory.
        /// </summary>
        /// <value>
        /// The data directory.
        /// </value>
        public DirectoryInfo DataDirectory { get; }

        /// <summary>
        /// Gets the temporary directory.
        /// </summary>
        /// <value>
        /// The temporary directory.
        /// </value>
        public DirectoryInfo TempDirectory { get; }

        private readonly CancellationToken _token;
        private int _exitCode = 0;
        private ILogger? _logger => LogService?.GetContextLogger<IHost>();

        public event EventHandler<int>? BeforeExit;
        public event EventHandler<int>? Exit;

        public IActorService? ActorService { get; private set; }
        public IConfigService? ConfigService { get; private set; }
        public IEventService? EventService { get; private set; }
        public ILogService? LogService { get; private set; }
        public IPluginService? PluginService { get; private set; }

        public MopHost(HostProperties p, CancellationToken token)
        {
            Info = BuildHostInfo(p.Id, p.Name);
            DataDirectory = new DirectoryInfo(p.DataDirectory);
            TempDirectory = new DirectoryInfo(p.TempDirectory);
            _token = token;
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public async Task<int> Start()
        {
            _logger?.Information("Starting MOP host with Name {@Name} and Id {@Id}", Info.Name, Info.Id);
            await LoadLocalPlugins();
            await Task.Run(() 
                => { while (!_token.IsCancellationRequested) { } });
            return _exitCode;
        }

        public void Terminate(int exitCode)
        {
            _exitCode = exitCode;
            BeforeExit?.Invoke(this, exitCode);
            Exit?.Invoke(this, exitCode);
        }

        public void SetActorService(IActorService actorService, bool replace = false)
            => ActorService = GetValue(replace, ActorService, actorService);
        public void SetConfigService(IConfigService configService, bool replace = false)
            => ConfigService = GetValue(replace, ConfigService, configService);
        public void SetEventService(IEventService eventService, bool replace = false)
            => EventService = GetValue(replace, EventService, eventService);
        public void SetLogService(ILogService logService, bool replace = false)
            => LogService = GetValue(replace, LogService, logService);
        public void SetPluginService(IPluginService pluginService, bool replace = false)
            => PluginService = GetValue(replace, PluginService, pluginService);

        private T GetValue<T>(bool replace, T instance, T value)
            => (replace || instance is null) ? value : instance;

        private async Task LoadLocalPlugins()
        {
            if (PluginService is null)
                throw new ArgumentNullException("Plugin service must be defined");
            var dirPath = Path.Combine(Environment.CurrentDirectory, "Plugins");
            await PluginService.AddPluginsFolder(new DirectoryInfo(dirPath));
            await PluginService.Load();
        }

        public static async Task<MopHost> BuildHost(string[] args, CancellationToken token)
        {
            var props = await HostPropertiesService.LoadHostProperties(args);
            var host = new MopHost(props, token);
            host.SetLogService(new LogService(props));
            host.SetConfigService(await ConfigServiceBuilder.Build(host));
            host.SetActorService(new ActorService(host, props));
            host.SetEventService(new EventService(host));
            host.SetPluginService(new PluginService(host));
            return host;
        }
    }
}
