using MOP.Core.Domain.Events;
using System;
using System.Collections.Generic;

namespace MOP.Host.Events
{
    /// <summary>
    /// Represents a subscription to a list of events
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IEventSubscription : IDisposable
    {
        IEnumerable<ulong> EventTypes { get; }
        Action<IEvent> Handler { get; }
        Guid Id { get; }

        event EventHandler<Guid>? OnDispose;
    }
}