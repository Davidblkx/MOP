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
        Guid Emit(string type);

        /// <summary>
        /// Emits the specified type.
        /// </summary>
        /// <typeparam name="T">body type</typeparam>
        /// <param name="type">The type.</param>
        /// <param name="body">The body.</param>
        /// <returns>The event id</returns>
        Guid Emit<T>(string type, T body);

        /// <summary>
        /// Subscribes to all events using the specified handler.
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <returns>Disposable instance to allow to unsubscribe</returns>
        IDisposable Subscribe(Action<IEvent> handler);

        /// <summary>
        /// Subscribes to events of the specified types using the specified handler.
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <param name="types">The types to subscribe to.</param>
        /// <returns>Disposable instance to allow to unsubscribe</returns>
        IDisposable Subscribe(Action<IEvent> handler, params string[] types);
    }
}
