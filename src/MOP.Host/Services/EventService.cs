using Akka.Actor;
using Optional;
using MOP.Core.Domain.Events;
using MOP.Core.Domain.Host;
using MOP.Core.Services;
using MOP.Host.Events;
using Serilog;
using System;
using System.Threading.Tasks;
using Optional.Unsafe;

using static MOP.Core.Optional.StaticOption;

namespace MOP.Host.Services
{
    internal class EventService : IEventService
    {
        private readonly ILogger _log;
        private readonly IActorRef _eventsActor;

        public EventService(IHost host)
        {
            if (host.LogService is null)
                throw new ArgumentNullException("Can't initialize IEventService before ILogService");
            if (host.ActorService is null)
                throw new ArgumentNullException("Can't initialize IEventService before IActorService");

            _log = host.LogService.GetContextLogger<IEventService>();
            _eventsActor = InitActorFactor(host)
                .ValueOrFailure("Failed to initialize events actor");

        }

        public Task<Guid> Emit(string type)
            => Emit(type, new Unit());

        public Task<Guid> Emit<T>(string type, T body)
        {
            _log.Information("New event: {@Type} {@Body}", type, body);
            var cmd = EventCommand.Create(type, body);
            _eventsActor.Tell(cmd);
            return Task.Run(() => cmd.Event.Id);
        }

        public void ReplayEvents(Guid? startEventGuid)
        {
            Some(startEventGuid).Match(
                some: e => _log.Information("Replaying events from id: {@E}", e),
                none: () => _log.Information("Replaying all events"));

            var cmd = new ReplayCommand(new string[0], startEventGuid);
            _eventsActor.Tell(cmd);
        }

        public void ReplayEvents(string[] types, Guid? startEventGuid)
        {
            Some(startEventGuid).Match(
                some: e => _log.Information("Replaying events for type {@Types} starting at {@E}", e, types),
                none: () => _log.Information("Replaying all events for type {@Type}", types));

            var cmd = new ReplayCommand(new string[0], startEventGuid);
            _eventsActor.Tell(cmd);
        }

        public async Task<Option<IDisposable>> Subscribe(Action<IEvent> handler, params string[] types)
        {
            return (await AskSubscribe(handler, types)) switch
            {
                IDisposable e => Some(e),
                _ => None<IDisposable>()
            };
        }

        private Task<object> AskSubscribe(Action<IEvent> handler, params string[] types)
            => _eventsActor.Ask(new SubscribeCommand(handler, types));

        private Option<IActorRef> InitActorFactor(IHost host)
        {
            var factory = new EventsActorFactory(host, "events");
            host.ActorService?.AddActorFactory(factory);
            return host.ActorService?
                .GetActorOf(factory.ActorRefName)
                ?? None<IActorRef>();
        }
    }
}
