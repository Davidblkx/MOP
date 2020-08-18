using MOP.Infra.Domain.Events;
using MOP.Infra.Domain.Host;
using MOP.Infra.Tools;
using MOP.Infra.Services;
using MOP.Infra.Optional;
using Serilog;
using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace MOP.Infra.Domain.Plugins
{
    /// <summary>
    /// Default implementation for IPlugin
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="MOP.Infra.Domain.Plugins.IPlugin" />
    public abstract class Plugin<T> : IPlugin
    {
        public IPluginInfo Info { get; }

        protected IHost Host => Static.ThrowOnNull(_host);
        protected ILogger Logger => Static.ThrowOnNull(_log);
        protected IEventService Events
            => Static.ThrowOnNull(_events);
        protected IActorService Actors
            => Static.ThrowOnNull(_host?.ActorService);
        protected IConfigService Config
            => Static.ThrowOnNull(_host?.ConfigService);
        protected IPluginService Plugins
            => Static.ThrowOnNull(_host?.PluginService);

        private IHost? _host { get; set; }
        private ILogger? _log { get; set; }
        private IEventService? _events { get; set; }

        private readonly List<IDisposable> _disposables
            = new List<IDisposable>();

        public Plugin(string guid, string name, ulong priority)
        {
            Info = InfoBuilder.BuildPluginInfo(Guid.Parse(guid), name);
            Info.Priority = priority;
        }

        public virtual Task PreLoad(IHost host)
            => Task.Run(() => _host = host);

        public virtual async Task<bool> Initialize()
        {
            _log = Host.LogService?.GetContextLogger<T>();
            _events = Host.EventService;
            Logger.Debug("Initializing {Name} with priority {Priority}", Info.Name, Info.Priority);
            try
            {
                var res = await Start();
                var status = res ? "Success" : "Failed";
                Logger.Debug("Initialization has ended with status: {status}", status);
                return res;
            } 
            catch (Exception ex)
            {
                Logger.Error(ex, "Error loading plugin {Name}", Info.Name);
                return false;
            }
        }

        public virtual void Dispose()
        {
            _disposables.ForEach(e => e.Dispose());
        }

        /// <summary>
        /// Starts this instance, runs during initialization.
        /// </summary>
        /// <returns></returns>
        protected virtual Task<bool> Start()
            => Task.Run(() => true);

        /// <summary>
        /// Subscribes the specified handler.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="handler">The handler.</param>
        /// <param name="types">The types.</param>
        /// <returns></returns>
        protected IDisposable Subscribe<TEvent>(Action<TEvent> handler, params string[] types)
            where TEvent : IEvent
        {
            var res = Events.Subscribe(ev => { if (ev is TEvent e) handler(e); }, types);
            _disposables.Add(res);
            return res;
        }
    }
}
