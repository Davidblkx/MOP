using MOP.Core.Domain.Host;
using MOP.Core.Services;
using MOP.Host.Domain;
using MOP.Host.Services;
using MOP.Core.Domain.Plugins;
using MOP.Core.Infra;
using MOP.Host.Factories;
using MOP.Host.Plugins;
using MOP.Core.Infra.Tools;
using MOP.Core.Infra.Extensions;
using Serilog;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using static MOP.Core.Tools.InfoBuilder;

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

        private readonly MopLifeService _lifeHandler;
        private int _exitCode = 0;
        private readonly IInjectorService _injector;
        private ILogger _logger => _injector
            .GetService<ILogService>()
            .GetContextLogger<IHost>();

        public event EventHandler<int>? BeforeExit;
        public event EventHandler<int>? Exit;

        public MopHost(HostProperties p, MopLifeService life, IInjectorService injector)
        {
            Info = BuildHostInfo(p.Id, p.Name);
            DataDirectory = new DirectoryInfo(p.DataDirectory);
            TempDirectory = new DirectoryInfo(p.TempDirectory);
            _lifeHandler = life;
            _injector = injector;
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public async Task<int> Start()
        {
            _logger.Information("Starting MOP host with Name {@Name} and Id {@Id}", Info.Name, Info.Id);
            await LoadLocalPlugins();
            await _lifeHandler.WaitForExit();
            return _exitCode;
        }

        public void Terminate(int exitCode)
        {
            _exitCode = exitCode;
            BeforeExit?.Invoke(this, exitCode);
            Exit?.Invoke(this, exitCode);
        }

        private async Task LoadLocalPlugins()
        {
            var _events = _injector.GetService<IEventService>();
            var _plugins = _injector.GetService<IPluginService>();

            var dir = PathTools.GetStartDirectory().RelativeDirectory("Plugins");
            _plugins.AddPluginsFolder(dir);
            await _plugins.Load();

            await _events.Emit("Initial plugins loaded");
        }

        public static async Task<MopHost> BuildHost(string[] args, CancellationToken token)
        {
            var props = await HostPropertiesFactory.BuildPropertiesAsync(args);
            var injector = new InjectorService();
            var mainSystem = ActorSystemFactory.BuildFrom(props);

            injector.RegisterService(() => props, LifeCycle.Singleton);
            injector.RegisterService(() => new MopLifeService(token), LifeCycle.Singleton);
            injector.RegisterService(() => mainSystem, LifeCycle.Singleton);
            injector.RegisterService<IInjectorService>(() => injector, LifeCycle.Singleton);
            injector.RegisterService<IRIPService, RIPService>(LifeCycle.Singleton);
            injector.RegisterService<IHost, MopHost>(LifeCycle.Singleton);
            injector.RegisterService<ILogService, LogService>(LifeCycle.Singleton);
            injector.RegisterService<IConfigService, ConfigService>(LifeCycle.Singleton);
            injector.RegisterService<IEventService, EventService>(LifeCycle.Singleton);
            injector.RegisterService<PluginLoader>(LifeCycle.Transient);
            injector.RegisterService<IPluginService, PluginService>(LifeCycle.Singleton);
            injector.RegisterService<AssemblyResolver>(LifeCycle.Transient);

            if (injector.GetService<AssemblyResolver>() is AssemblyResolver r)
                r.RegisterAppDomain(AppDomain.CurrentDomain);

            if (injector.GetService<IHost>() is MopHost host)
                return host;

            throw new Exception("Failed to instantiate host");
        }
    }
}
