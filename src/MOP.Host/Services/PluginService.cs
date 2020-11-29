using Akka.Util.Internal;
using MOP.Core.Domain.Plugins;
using MOP.Core.Services;
using MOP.Host.Plugins;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace MOP.Host.Services
{
    internal class PluginService : IPluginService
    {
        private readonly IInjectorService _injector;
        private readonly AssemblyLoader _loader;
        private readonly SynchronizedCollection<Type> _pending;
        private readonly List<IPlugin> _ready;
        private readonly List<IPlugin> _failed;

        public PluginService(IInjectorService injector, ILogService logService)
        {
            _injector = injector;
            _loader = new AssemblyLoader(logService);
            _pending = new SynchronizedCollection<Type>();
            _ready = new List<IPlugin>();
            _failed = new List<IPlugin>();
        }

        public void AddPlugins(params Type[] pluginList)
        {
            pluginList.ForEach(e => _pending.Add(e));
        }

        public void AddPluginsAssembly(Assembly assembly)
        {
            AddPlugins(assembly.GetTypes());
        }

        public void AddPluginsFolder(DirectoryInfo info)
        {
            if (!info.Exists) return;

            var files = info.GetFiles("*.dll", SearchOption.AllDirectories);
            foreach (var f in files)
            {
                _loader.LoadAssemblyFromFile(f)
                    .MatchSome(a => AddPluginsAssembly(a));
            }
        }

        public IEnumerable<IPlugin> GetFailedPlugins() => _failed;

        public IEnumerable<IPlugin> GetLoadedPlugins() => _ready;

        public IEnumerable<Type> GetPending() => _pending;

        public async Task Load()
        {
            var loader = _injector.GetService<PluginLoader>();
            loader.AddPlugin(_pending);
            _pending.Clear();

            await loader.Load();

            _ready.AddRange(loader.Success);
            _failed.AddRange(loader.Failed);
        }
    }
}
