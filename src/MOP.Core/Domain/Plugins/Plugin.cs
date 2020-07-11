using MOP.Core.Domain.Events;
using MOP.Core.Domain.Host;
using MOP.Core.Helpers;
using MOP.Core.Optional;
using MOP.Core.Services;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MOP.Core.Domain.Plugins
{
    /// <summary>
    /// Default implementation for IPlugin
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="MOP.Core.Domain.Plugins.IPlugin" />
    public abstract class Plugin<T> : IPlugin
    {
        public IPluginInfo Info { get; }
        public IHost Host => StaticOption.ThrowOnNull(_host);
        public ILogger Logger => StaticOption.ThrowOnNull(_log);
        public IEventService Events
            => StaticOption.ThrowOnNull(_events);

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
            Logger.Debug("Initializing with priority {Priority}", Info.Priority);
            try
            {
                var res = await Start();
                Logger.Debug("Initialization has ended with status: {Res}", res);
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
