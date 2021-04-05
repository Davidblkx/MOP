using MOP.Terminal.Actors.Helpers;
using MOP.Terminal.Infra;
using MOP.Terminal.Models;
using MOP.Terminal.Models.Messages;

namespace MOP.Terminal.Actors
{
    internal class EndActor : BaseMopActor
    {
        public const string ACTOR_NAME = "end";

        private readonly StartupArgs _startup;

        public EndActor()
        {
            _startup = DependencyInjector.GetInstance<StartupArgs>();
            Receive<bool>(HandleTermination);
        }

        /// <summary>
        /// Handles the termination.
        /// </summary>
        /// <param name="force">if set to <c>true</c> force termination.</param>
        public void HandleTermination(bool force)
        {
            // If cancellation was requested, just stop
            if (AppState.Life.IsCancellationRequested) { return; }

            // If force termination or is not interactive mode, terminate
            if (force || !_startup.Interactive)
            {
                AppState.Life.Cancel();
                return;
            }

            // Send request to read a new user input
            TellActor(ActorPaths.InActorMeta, InActorMessages.READ_INPUT);
        }
    }
}
