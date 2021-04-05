using Akka.Actor;
using MOP.Terminal.Models.Messages;

namespace MOP.Terminal.Actors
{
    internal class SupervisorActor : ReceiveActor
    {
        public const string ACTOR_NAME = "MAIN_SUPERVISOR";

        public readonly IActorRef _inActor;

        public SupervisorActor()
        {
            _inActor = Context.ActorOf<InActor>(InActor.ACTOR_NAME);
            Context.ActorOf<EndActor>(EndActor.ACTOR_NAME);
            Context.ActorOf<OutActor>(OutActor.ACTOR_NAME);

            Receive<string[]>(OnInputStart);
        }

        /// <summary>
        /// Called when input starts, forward the start arguments
        /// </summary>
        /// <param name="args">The arguments.</param>
        private void OnInputStart(string[] args) => _inActor.Tell(InActorMessages.InvokeArgs(args));
    }
}
