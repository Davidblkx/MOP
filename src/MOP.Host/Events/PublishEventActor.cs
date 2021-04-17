using Akka.Actor;
using Akka.Cluster.Tools.PublishSubscribe;

namespace MOP.Core.Domain.Events
{
    public class PublishEventActor : ReceiveActor
    {
        public const string ActorName = "event-publish";

        public PublishEventActor()
        {
            var mediator = DistributedPubSub
                .Get(Context.System).Mediator;

            Receive<Event>(e =>
            {
                mediator.Tell(new Publish(e.Type, e));
            });
        }

        public static Props GetProps() =>
            Props.Create<PublishEventActor>();
    }
}
