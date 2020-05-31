using Akka.Actor;
using Optional;

namespace MOP.Core.Domain.Actors
{
    /// <summary>
    /// Exposes a factory method for a <see cref="IActorRef"/>
    /// </summary>
    public interface IActorFactory
    {
        /// <summary>
        /// Gets the name of the actor reference.
        /// </summary>
        /// <value>
        /// The name of the actor reference.
        /// </value>
        string ActorRefName { get; }

        /// <summary>
        /// Tells the system when a actor instance should be created
        /// </summary>
        /// <value>
        /// The type of the instance.
        /// </value>
        IActorRefInstanceType InstanceType { get; }

        /// <summary>
        /// Builds the actor reference.
        /// </summary>
        /// <param name="actorSystem">The actor system.</param>
        /// <returns></returns>
        Option<IActorRef> BuildActorRef(ActorSystem actorSystem);
    }

    /// <summary>
    /// Tells the system when a actor instance should be created
    /// </summary>
    public enum IActorRefInstanceType
    {
        /// <summary>
        /// It's only created once
        /// </summary>
        Singleton = 0,
        /// <summary>
        /// It's created at every request
        /// </summary>
        Transient = 1,
    }
}