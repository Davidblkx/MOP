namespace MOP.Terminal.Models.Messages
{
    internal static class InActorMessages
    {
        /// <summary>
        /// Tell actor to read user input
        /// </summary>
        public const bool READ_INPUT = true;

        /// <summary>
        /// Message to tell actor to invoke arguments
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>the message</returns>
        public static InActorArguments InvokeArgs(string[] args) => new(args);
    }

    internal class InActorArguments
    {
        public string[] Args { get; }

        public InActorArguments(string[] args)
        { Args = args; }
    }
}
