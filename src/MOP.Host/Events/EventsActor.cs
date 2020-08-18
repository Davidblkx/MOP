using Akka.Actor;
using MOP.Infra.Domain.Events;

namespace MOP.Host.Events
{
    /// <summary>
    /// Actor responsible to handle events and subscriptions
    /// </summary>
    /// <seealso cref="Akka.Actor.ReceiveActor" />
    internal class EventsActor : ReceiveActor
    {
        private readonly EventSubscriptionHandler _handler;

        public EventsActor(EventSubscriptionHandler handler)
        {
            _handler = handler;
            DeclareReceive();
        }

        private void DeclareReceive()
        {
            Receive<SubscribeCommand>(cmd => 
                Sender.Tell(_handler.Subscribe(cmd)));
            Receive<ReplayCommand>(cmd => 
                _handler.Emit(cmd.StartId, cmd.EventTypes));
            Receive<EventCommand>(cmd => _handler.Emit(cmd.Event));
        }

        public static Props WithProps(EventSubscriptionHandler handler)
            => Props.Create(() => new EventsActor(handler));
    }
}
