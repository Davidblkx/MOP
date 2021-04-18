using System;
using System.Linq;
using System.Reactive.Linq;

namespace MOP.Core.Domain.Events
{
    public static class EventsExtensions
    {
        public static IEvent<K> Cast<T, K>(this IEvent<T> @event)
            => new Event<T>(@event).Cast<K>();

        public static IEvent<K> Cast<K>(this IEvent<object> @event)
            => new Event<object>(@event).Cast<K>();

        public static IObservable<IEvent> OfType(this IObservable<IEvent> observable, params string[] types)
            => observable.Where(e => types.Contains(e.Type));
    }
}
