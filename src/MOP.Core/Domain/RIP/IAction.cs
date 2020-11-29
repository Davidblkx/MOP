namespace MOP.Core.Domain.RIP
{
    public interface IAction
    {
        /// <summary>
        /// Name to call this action
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        string? Description { get; }

        /// <summary>
        /// Gets the return description.
        /// </summary>
        /// <value>
        /// The return description.
        /// </value>
        string? ReturnDescription { get; }

        /// <summary>
        /// Gets the type of the argument.
        /// </summary>
        /// <value>
        /// The type of the argument.
        /// </value>
        string ArgumentType { get; }

        /// <summary>
        /// Gets the argument JSON schema, to allow user validation and documentation.
        /// </summary>
        /// <value>
        /// The argument schema.
        /// </value>
        string ArgumentSchema { get; }

        /// <summary>
        /// Gets the type of the return.
        /// </summary>
        /// <value>
        /// The type of the return.
        /// </value>
        string ReturnType { get; }

        /// <summary>
        /// Gets the return JSON schema, to allow user validation and documentation.
        /// </summary>
        /// <value>
        /// The return schema.
        /// </value>
        string ReturnSchema { get; }
    }
}
