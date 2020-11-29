using MOP.Core.Domain.Plugins;
using MOP.Core.Infra.Tools;
using MOP.Core.Services;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Optional;
using Akka.Util.Internal;
using Optional.Unsafe;

using static MOP.Core.Infra.Optional.Static;

namespace MOP.Host.Plugins
{
    /// <summary>
    /// Instantiate and initialize plugins
    /// </summary>
    public class PluginLoader
    {
        private readonly List<Type> _pendingPlugins = new List<Type>();
        private readonly List<Type> _pendingServices = new List<Type>();
        private readonly List<IPlugin> _failed = new List<IPlugin>();
        private readonly List<IPlugin> _success = new List<IPlugin>();
        private readonly IInjectorService _injector;
        private readonly ILogger _logger;
        private readonly IRIPService _rip;

        public IEnumerable<IPlugin> Failed => _failed;
        public IEnumerable<IPlugin> Success => _success;

        public bool IsCompleted { get; private set; }

        public PluginLoader(IInjectorService injector, ILogService log, IRIPService rip)
        {
            IsCompleted = false;
            _injector = injector;
            _logger = log.GetContextLogger<IPluginService>();
            _rip = rip;
        }

        public void AddPlugin(Type type, bool warn = true)
        {
            if (TypeTools.CanInstantiate<IPlugin>(type))
            {
                if (TypeTools.ShouldBeIgnored(type))
                    return;

                if (TypeTools.CanInstantiate<IServicePlugin>(type))
                    _pendingServices.Add(type);
                else
                    _pendingPlugins.Add(type);
                return;
            }

            if (warn) _logger.Warning($"Can't load IPlugin from {type.FullName}");
        }

        public void AddPlugin(IEnumerable<Type> types, bool warn = true)
            => types.ForEach(e => AddPlugin(e, warn));

        public async Task Load()
        {
            if (IsCompleted) 
                throw new InvalidOperationException("Can only be called once");

            await LoadServices();
            await LoadPlugins();

            IsCompleted = true;
        }

        private async Task LoadServices()
        {
            var services = Instatiate<IServicePlugin>(_pendingServices);
            foreach (var s in services)
                if (await s.Initialize())
                {
                    s.Implements.ForEach(e => _injector.RegisterService(e, s.GetType(), s.LifeCycle));
                    _rip.Register(s.GetType(), true);
                    _success.Add(s);
                    _logger.Debug($"Service Plugin loaded: {s.Info.Name}");
                }
                else
                {
                    _failed.Add(s);
                    _logger.Warning($"Failed to load Service Plugin: {s.Info.Name}");
                }
        }

        private async Task LoadPlugins()
        {
            var plugins = Instatiate<IPlugin>(_pendingPlugins);
            foreach (var p in plugins)
                if (await p.Initialize())
                {
                    _success.Add(p);
                    _logger.Debug($"Plugin loaded: {p.Info.Name}");
                }
                else
                {
                    _failed.Add(p);
                    _logger.Warning($"Failed to load Plugin: {p.Info.Name}");
                }
        }

        private IEnumerable<T> Instatiate<T>(IEnumerable<Type> list) where T : IPlugin
        {
            list.ForEach(e => _injector.RegisterService(e));
            return list
                .Select(e => Instatiate<T>(e))
                .Where(e => e.HasValue)
                .Select(e => e.ValueOrDefault())
                .OrderBy(e => e.Info.Priority);
        }

        private Option<T> Instatiate<T>(Type type) where T : IPlugin
        {
            try
            {
                var instance = _injector.GetService(type);
                if (instance is T valid) return Some(valid);
            } 
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error instantiating {type.FullName}");
            }
            _logger.Debug($"Cant instantiate type {type.FullName}");
            return None<T>();
        }
    }
}
