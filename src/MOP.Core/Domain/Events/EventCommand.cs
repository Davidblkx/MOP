namespace MOP.Core.Domain.Events
{
    /// <summary>
    /// Command to create and emit an event
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EventCommand<T>
    {
        /// <summary>
        /// Gets the event that will be emited.
        /// </summary>
        /// <value>
        /// The event.
        /// </value>
        public IEvent<T> Event { get; }

        internal EventCommand(string type, T body)
        {
            Event = new Event<T>(type, body);
        }
    }

    /// <summary>
    /// Helper to instaciate event commands
    /// </summary>
    public static class EventCommand
    {
        public static EventCommand<T> Create<T>(string type, T body)
            => new EventCommand<T>(type, body);

        public static EventCommand<Unit> Create(string type)
            => new EventCommand<Unit>(type, default);
    }
}
