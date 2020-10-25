namespace MOP.Core.Domain.Api
{
    /// <summary>
    /// Message to send to API actor to be serialized into an MOP actor system message
    /// </summary>
    public class ApiCommand
    {
        /// <summary>
        /// Gets or sets message the target, usually its an actor path.
        /// </summary>
        /// <value>
        /// The target.
        /// </value>
        public string Target { get; set; }

        /// <summary>
        /// Gets or sets the assembly qualified name for the message type.
        /// </summary>
        /// <value>
        /// The type of the message.
        /// </value>
        public string MessageType { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// Message should be serialized in JSON
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; }

        public ApiCommand(string target, string type, string message)
        {
            Target = target;
            MessageType = type;
            Message = message;
        }

        public static ApiCommand Create(string target, string type, string message)
            => new ApiCommand(target, type, message);
    }
}
