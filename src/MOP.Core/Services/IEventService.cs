using MOP.Core.Domain.Events;
using System;

namespace MOP.Core.Services
{
    /// <summary>
    /// Service to emit and subscribe to events
    /// </summary>
    public interface IEventService
    {
        /// <summary>
        /// Emits the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The event id</returns>
        Guid Emit(string type, bool global = false);

        /// <summary>
        /// Emits the specified type.
        /// </summary>
        /// <typeparam name="T">body type</typeparam>
        /// <param name="type">The type.</param>
        /// <param name="body">The body.</param>
        /// <returns>The event id</returns>
        Guid Emit<T>(string type, T body, bool global = false);

        Guid Emit(IEvent @event, bool global = false);

        IObservable<IEvent> Events { get; }
    }
}
