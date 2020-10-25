namespace MOP.Core.Domain.Api
{
    /// <summary>
    /// Represents an action that can be invoked by the APIActor
    /// </summary>
    public class ApiAction
    {
        /// <summary>
        /// Gets or sets the action name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description for current action.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the return description.
        /// </summary>
        /// <value>
        /// The return description.
        /// </value>
        public string? ReturnDescription { get; set; }

        /// <summary>
        /// Gets or sets the type name of the message.
        /// </summary>
        /// <value>
        /// The type of the message.
        /// </value>
        public string MessageType { get; set; } = "System.Void";
        /// <summary>
        /// Gets or sets the message type JSON schema.
        /// </summary>
        /// <value>
        /// The message type schema.
        /// </value>
        public string MessageTypeSchema { get; set; } = "";

        /// <summary>
        /// Gets or sets the type of the return.
        /// </summary>
        /// <value>
        /// The type of the return.
        /// </value>
        public string ReturnType { get; set; } = "System.Void";
        /// <summary>
        /// Gets or sets the return type JSON schema.
        /// </summary>
        /// <value>
        /// The return type schema.
        /// </value>
        public string ReturnTypeSchema { get; set; } = "";

        public ApiAction(string name)
        {
            Name = name;
        }
    }
}
