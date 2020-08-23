using MOP.Infra.Domain.Host;
using MOP.Infra.Domain.Plugins;
using MOP.Infra.Services;
using MOP.Host.Plugins;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MOP.Core.Infra.Extensions;

namespace MOP.Host.Services
{
    internal class PluginService : IPluginService
    {
        private readonly ILogService _logService;
        private readonly ILogger _log;
        private readonly AssemblyLoader _loader;
        private readonly List<IPlugin> _pending;
        private readonly List<IPlugin> _ready;
        private readonly List<IPlugin> _failed;

        public PluginService(ILogService log)
        {
            _logService = log;
            _log = _logService.GetContextLogger<IPluginService>();
            _loader = new AssemblyLoader(_logService);
            _pending = new List<IPlugin>();
            _ready = new List<IPlugin>();
            _failed = new List<IPlugin>();
        }

        public async Task AddPlugins(params IPlugin[] pluginList)
        {
            foreach (var p in pluginList)
            {
                await AddPlugin(p);
            }
        }

        public async Task AddPluginsAssembly(Assembly assembly)
        {
            var plugin = _loader.LoadPluginFromAssembly(assembly);
            await plugin.AwaitSome(p => AddPlugins(p));
        }

        public async Task AddPluginsFolder(DirectoryInfo info)
        {
            if (!info.Exists) return;

            var files = info.GetFiles("*.dll", SearchOption.AllDirectories);
            foreach (var f in files)
            {
                await _loader.LoadAssemblyFromFile(f)
                    .AwaitSome(a => AddPluginsAssembly(a));
            }
        }

        public IEnumerable<IPlugin> GetFailedPlugins() => _failed;

        public IEnumerable<IPlugin> GetLoadedPlugins() => _ready;

        public IEnumerable<IPlugin> GetPendingPlugins() => _pending;

        public async Task Load()
        {
            var listToLoad = new List<IPlugin>(_pending);
            _pending.Clear();
            _log.Information("Starting loading {@Count} pending plugins", listToLoad.Count);
            foreach (var p in listToLoad.OrderBy(e => e.Info.Priority))
            {
                if (await TryLoadingPlugin(p))
                    PluginLoadingSuccess(p);
                else
                    PluginLoadingFail(p);
            }
        }

        private async Task AddPlugin(IPlugin p)
        {
            if (await TryAddPlugin(p))
                _log.Information("Preloaded plugin {@Name}", p.Info.Name);
        }

        private async Task<bool> TryLoadingPlugin(IPlugin p)
        {
            try
            {
                return await p.Initialize();
            }
            catch (Exception ex)
            {
                _log.Error("Can't load plugin {@Name}", p.Info.Name);
                _log.Debug(ex, "Error loading a plugin");
                return false;
            }
        }

        private async Task<bool> TryAddPlugin(IPlugin p)
        {
            throw new NotImplementedException();
        }

        private void PluginLoadingSuccess(IPlugin p)
        {
            _log.Information("Plugin loaded {@Name}", p.Info.Name);
            _ready.Add(p);
        }

        private void PluginLoadingFail(IPlugin p)
        {
            _log.Warning("Fail to start plugin {@Name}", p.Info.Name);
            _failed.Add(p);
        }
    }
}
