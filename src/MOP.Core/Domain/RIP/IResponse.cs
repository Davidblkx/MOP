namespace MOP.Core.Domain.RIP
{
    /// <summary>
    /// Response message to ICall
    /// </summary>
    public interface IResponse
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="IResponse"/> is valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if valid; otherwise, <c>false</c>.
        /// </value>
        bool Valid { get; }

        /// <summary>
        /// Gets the error details.
        /// </summary>
        /// <value>
        /// The error.
        /// </value>
        (int code, string message)? Error { get; }

        /// <summary>
        /// Gets the response body.
        /// </summary>
        /// <value>
        /// The body.
        /// </value>
        object? Body { get; }
    }
}
