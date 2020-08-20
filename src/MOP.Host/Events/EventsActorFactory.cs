using Akka.Actor;
using MOP.Core.Infra.Extensions;
using MOP.Infra.Domain.Actors;
using MOP.Infra.Domain.Host;
using Optional;
using System;
using static MOP.Infra.Domain.Actors.IActorRefInstanceType;
using static MOP.Core.Infra.Optional.Static;

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

        public string ActorRefName { get; }
        public IActorRefInstanceType InstanceType => Singleton;

        public EventsActorFactory(IHost host, string filename)
        {
            _host = host;
            ActorRefName = "Events-" + _host.Info.Id.ToString();
            _filename = filename;
        }

        public Option<IActorRef> BuildActorRef(ActorSystem actorSystem)
        {
            var _logService = _host.LogService;
            if (_logService is null) return None<IActorRef>();
            try
            {
                var dbPath = _host.DataDirectory.RelativeFile($"{_filename}.db");
                var storage = new EventStorage(dbPath, _logService);
                var handler = new EventSubscriptionHandler(storage, _logService);
                var actor = actorSystem.ActorOf(EventsActor.WithProps(handler), ActorRefName);
                return Some(actor);
            } catch (Exception e)
            {
                _logService.GetContextLogger<EventsActorFactory>()
                    .Error(e, "Error initializing actor for events");
                return None<IActorRef>();
            }
        }
    }
}
