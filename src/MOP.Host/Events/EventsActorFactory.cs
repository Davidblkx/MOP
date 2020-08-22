using Akka.Actor;
using MOP.Core.Infra.Extensions;
using MOP.Infra.Domain.Actors;
using MOP.Infra.Domain.Host;
using Optional;
using System;
using static MOP.Infra.Domain.Actors.IActorRefInstanceType;
using static MOP.Core.Infra.Optional.Static;
using Serilog;
using MOP.Infra.Services;

namespace MOP.Host.Events
{
    /// <summary>
    /// Factory to create the Events actor
    /// </summary>
    /// <seealso cref="MOP.Infra.Domain.Actors.IActorFactory" />
    internal class EventsActorFactory : IActorFactory
    {
        private readonly IHost _host;
        private readonly string _filename;
        private readonly ILogger _logger;
        private readonly ILogService _logService;

        public string ActorRefName { get; }
        public IActorRefInstanceType InstanceType => Singleton;

        public EventsActorFactory(IHost host, string filename, ILogService log)
        {
            _host = host;
            _logService = log;
            _filename = filename;
            _logger = log.GetContextLogger<EventsActorFactory>();
            ActorRefName = "Events-" + _host.Info.Id.ToString();
        }

        public Option<IActorRef> BuildActorRef(ActorSystem actorSystem)
        {
            try
            {
                var dbPath = _host.DataDirectory.RelativeFile($"{_filename}.db");
                var storage = new EventStorage(dbPath, _logService);
                var handler = new EventSubscriptionHandler(storage, _logService);
                var actor = actorSystem.ActorOf(EventsActor.WithProps(handler), ActorRefName);
                return Some(actor);
            } catch (Exception e)
            {
                _logger.Error(e, "Error initializing actor for events");
                return None<IActorRef>();
            }
        }
    }
}
