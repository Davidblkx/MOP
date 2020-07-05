namespace MOP.Core.Domain.Events
{
    public static class EventsExtensions
    {
        public static IEvent<K> Cast<T, K>(this IEvent<T> @event)
            => new Event<T>(@event).Cast<K>();

        public static IEvent<K> Cast<K>(this IEvent<object> @event)
            => new Event<object>(@event).Cast<K>();
    }
}
