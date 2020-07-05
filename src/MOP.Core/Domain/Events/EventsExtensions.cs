namespace MOP.Core.Domain.Events
{
    public static class EventsExtensions
    {
        public static IEvent<K> Cast<T, K>(this IEvent<T> @event)
            => new Event<T>(@event).Cast<K>();
    }
}
