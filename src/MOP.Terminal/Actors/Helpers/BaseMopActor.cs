using Akka.Actor;
using System;
using System.Threading.Tasks;

namespace MOP.Terminal.Actors.Helpers
{
    internal abstract class BaseMopActor : ReceiveActor
    {
        /// <summary>
        /// Selects the actor.
        /// </summary>
        /// <param name="meta">The meta.</param>
        /// <returns></returns>
        protected ActorSelection SelectActor(ActorMetaData meta)
        {
            var actorSelection = Context.ActorSelection(meta.Path);
            if (Equals(actorSelection, ActorRefs.Nobody))
                throw new NullReferenceException($"Actor [{meta.Path}] is not available");
            return actorSelection;
        }

        /// <summary>
        /// Tells the actor.
        /// </summary>
        /// <param name="meta">The meta.</param>
        /// <param name="message">The message.</param>
        protected void TellActor(ActorMetaData meta, object message)
            => SelectActor(meta).Tell(message);

        /// <summary>
        /// Asks the actor.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="meta">The meta.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        protected Task<TResult> AskActor<TResult>(ActorMetaData meta, object message)
            => SelectActor(meta).Ask<TResult>(message);
    }
}
