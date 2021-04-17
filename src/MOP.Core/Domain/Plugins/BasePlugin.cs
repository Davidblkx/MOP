using MOP.Core.Services;
using MOP.Core.Domain.Events;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Linq;

namespace MOP.Core.Domain.Plugins
{
    public abstract class BasePlugin<T> : IPlugin
    {
        public IPluginInfo Info { get; }
        
        protected IInjectorService Injector { get; }
        protected ILogger Logger { get; }
        protected IEventService EventService { get; }
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
            EventService = Injector.GetService<IEventService>();
            _subscriptions = new List<IDisposable>();
        }

        public virtual void Dispose()
        {
            Logger.Debug("Disposing plugin {@FullName}", FullName);
            foreach (var disposable in _subscriptions)
                disposable.Dispose();
            GC.SuppressFinalize(this);
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
        protected void Subscribe(Action<IEvent> handler, params string[] types)
        {
            Logger.Debug("Subscribing {@FullName} to events: {types}", FullName, types);
            EventService.Events
                .Where(e => types.Contains(e.Type))
                .Subscribe(e => handler(e));
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
