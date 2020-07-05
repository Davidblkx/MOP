using MOP.Core.Domain.Events;
using System;
using System.Collections.Generic;

namespace MOP.Host.Events
{
    /// <summary>
    /// Subscription with dispose capability 
    /// </summary>
    /// <seealso cref="MOP.Host.Events.IEventSubscription" />
    internal class EventSubscription : IEventSubscription
    {
        public Guid Id => Guid.NewGuid();
        public Action<IEvent> Handler { get; }
        public IEnumerable<ulong> EventTypes { get; }

        public event EventHandler<Guid>? OnDispose;

        public EventSubscription(Action<IEvent> handler, IEnumerable<ulong> events)
        {
            Handler = handler;
            EventTypes = events;
        }

        public void Dispose()
            => OnDispose?.Invoke(this, Id);
    }
}
