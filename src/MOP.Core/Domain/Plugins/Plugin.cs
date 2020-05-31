using MOP.Core.Domain.Events;
using MOP.Core.Domain.Host;
using MOP.Core.Helpers;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MOP.Core.Domain.Plugins
{
    public abstract class Plugin<T> : IPlugin
    {
        public IPluginInfo Info { get; }
        public IHost Host { get; private set; }
        public ILogger Logger { get; private set; }

        private readonly List<IDisposable> _disposables
            = new List<IDisposable>();

        public Plugin(string guid, string name, ulong priority)
        {
            Info = InfoBuilder.BuildPluginInfo(Guid.Parse(guid), name);
            Info.Priority = priority;
        }

        public virtual async Task<bool> Initialize(IHost host)
        {
            Host = host;
            Logger = host.LogService.GetContextLogger<T>();
            Logger.Debug("Initializing with priority {Priority}", Info.Priority);
            var res = await Start();
            Logger.Debug("Initialization has ended with status: {Res}", res);
            return res;
        }

        public virtual Task AfterInit()
            => Task.Run(() => 
                Logger.Debug("Cleaning initialization"));

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
            var res = Host.EventService.Subscribe(ev => { if (ev is TEvent e) handler(e); }, types);
            _disposables.Add(res);
            return res;
        }
    }
}
