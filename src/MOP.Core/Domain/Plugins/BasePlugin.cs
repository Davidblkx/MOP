using MOP.Core.Services;
using MOP.Core.Domain.Events;
using MOP.Core.Domain.Plugins;
using MOP.Core.Services;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MOP.Core.Domain.Plugins
{
    public abstract class BasePlugin<T> : IPlugin
    {
        public IPluginInfo Info { get; }
        
        protected IInjectorService Injector { get; }
        protected ILogger Logger { get; }
        protected IEventService Events { get; }
        protected string FullName { get; }

        private List<IDisposable> _subscriptions;

        public BasePlugin(IInjectorService injector)
        {
            Info = BuildPluginInfo();
            FullName = $"{Info.Name}[{Info.Id}]";
            Injector = injector;
            Logger = Injector
                .GetService<ILogService>()
                .GetContextLogger<T>();
            Events = Injector.GetService<IEventService>();
            _subscriptions = new List<IDisposable>();
        }

        public virtual void Dispose()
        {
            Logger.Debug("Disposing plugin {@FullName}", FullName);
            foreach (var disposable in _subscriptions)
                disposable.Dispose();
        }

        public virtual async Task<bool> Initialize()
        {
            Logger.Debug("Initializing {@FullName}", FullName);
            try
            {
                var res = await OnInitAsync();
                if (res) Logger.Information("Plugin {@FullName} is ready", FullName);
                else Logger.Error("Plugin {@FullName} failed to start", FullName);
                return res;
            } 
            catch (Exception ex)
            {
                Logger.Error(ex, "Plugin {@FullName} failed to start", FullName);
                return false;
            }
        }

        /// <summary>
        /// Subscribes the specified handler.
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <param name="types">The types.</param>
        protected async Task Subscribe(Action<IEvent> handler, params string[] types)
        {
            Logger.Debug("Subscribing {@FullName} to events: {types}", FullName, types);
            var subs = await Events.Subscribe(handler, types);
            subs.MatchSome(d => _subscriptions.Add(d));
        }

        /// <summary>
        /// Called when initializing plugin.
        /// </summary>
        /// <returns></returns>
        protected virtual Task<bool> OnInitAsync()
            => Task.Run(() => true);

        /// <summary>
        /// Builds the plugin information.
        /// </summary>
        /// <returns></returns>
        protected abstract IPluginInfo BuildPluginInfo();
    }
}
