namespace MOP.Core.Domain.Events
{
    /// <summary>
    /// Command to create and emit an event
    /// </summary>
    public class EventCommand
    {
        /// <summary>
        /// Gets the event that will be emitted.
        /// </summary>
        /// <value>
        /// The event.
        /// </value>
        public IEvent<object> Event { get; private set; }

        internal EventCommand(IEvent<object> e) { Event = e; }

        public static EventCommand Create<T>(string type, T body)
            => new EventCommand(new Event<T>(type, body).Cast<object>());

        public static EventCommand Create(string type) => Create(type, new Unit());
    }
}
