using Akka.Actor;
using MOP.Core.Domain.Actors;
using MOP.Core.Domain.Host;
using MOP.Host.Helpers;
using Optional;
using System;
using static MOP.Core.Domain.Actors.IActorRefInstanceType;
using static MOP.Core.Helpers.NullHelper;

namespace MOP.Host.Events
{
    /// <summary>
    /// Factory to create the Events actor
    /// </summary>
    /// <seealso cref="MOP.Core.Domain.Actors.IActorFactory" />
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
