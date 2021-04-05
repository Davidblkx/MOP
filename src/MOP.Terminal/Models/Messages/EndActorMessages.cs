namespace MOP.Terminal.Models.Messages
{
    internal static class EndActorMessages
    {
        /// <summary>
        /// Forces termination
        /// </summary>
        public const bool TERMINATE = true;

        /// <summary>
        /// Check if should terminate the app or ask for new input
        /// </summary>
        public const bool VALIDATE = false;
    }
}
