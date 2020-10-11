using Optional;
using System;
using System.Collections.Generic;

namespace MOP.Core.Domain.Events
{
    /// <summary>
    /// Base event
    /// </summary>
    public interface IEvent : IComparable, IComparable<IEvent>, IEqualityComparer<IEvent>, IEquatable<IEvent>
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        Guid Id { get; }
        /// <summary>
        /// Gets the event type.
        /// </summary>
        /// <value>
        /// The event type.
        /// </value>
        string Type { get; }
        /// <summary>
        /// Gets the created date time in UTC.
        /// </summary>
        /// <value>
        /// The date time.
        /// </value>
        DateTime DateTime { get; }
    }

    /// <summary>
    /// Base event with body
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEvent<T> : IEvent
    {
        /// <summary>
        /// Gets the body.
        /// </summary>
        /// <value>
        /// The body.
        /// </value>
        Option<T> Body { get;  }
    }
}
