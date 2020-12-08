namespace MOP.Core.Domain.RIP
{
    /// <summary>
    /// Interface of messages to invoke a command action
    /// </summary>
    public interface ICall
    {
        /// <summary>
        /// Gets the command name.
        /// </summary>
        /// <value>
        /// The command.
        /// </value>
        string Command { get; }

        /// <summary>
        /// Gets the action name to invoke.
        /// </summary>
        /// <value>
        /// The action.
        /// </value>
        string Action { get; }

        /// <summary>
        /// Gets the action argument serialized in JSON.
        /// </summary>
        /// <value>
        /// The argument.
        /// </value>
        string? Argument { get; }
    }
}
