using Akka.Actor;
using Optional;
using MOP.Core.Domain.Events;
using MOP.Core.Services;
using Serilog;
using System;
using Optional.Unsafe;
using System.Reactive.Subjects;

using static MOP.Core.Infra.Optional.Static;

namespace MOP.Host.Services
{
    internal class EventService : IEventService
    {
        private readonly ILogger _log;
        private readonly IActorRef _eventsActor;
        private readonly Subject<IEvent> _subject;

        public IObservable<IEvent> Events => _subject;

        public EventService(ILogService log, ActorSystem actorSystem)
        {
            _log = log.GetContextLogger<IEventService>();
            _eventsActor = InitEventActor(actorSystem)
                .ValueOrFailure("Failed to initialize events actor");
            _subject = new Subject<IEvent>();
        }

        public Guid Emit(string type, bool global = false)
            => Emit(type, new Unit(), global);

        public Guid Emit<T>(string type, T body, bool global = false)
        {
            _log.Information("New event: {@Type} {@Body}", type, body);

            var @event = new Event<T>(type, body);
            _subject.OnNext(@event);

            if (global) _eventsActor.Tell(@event);

            return @event.Id;
        }

        private Option<IActorRef> InitEventActor(ActorSystem actorSystem)
        {
            try
            {
                return Some(actorSystem.ActorOf(
                    PublishEventActor.GetProps(),
                    PublishEventActor.ActorName
                   ));
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Failed to starts events actor");
                return None<IActorRef>();
            }
        }
    }
}
