using MOP.Core.Domain.Events;
using System;
using System.Threading.Tasks;

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
        Task<Guid> Emit(string type);

        /// <summary>
        /// Emits the specified type.
        /// </summary>
        /// <typeparam name="T">body type</typeparam>
        /// <param name="type">The type.</param>
        /// <param name="body">The body.</param>
        /// <returns>The event id</returns>
        Task<Guid> Emit<T>(string type, T body);

        /// <summary>
        /// Subscribes to events of the specified types using the specified handler.
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <param name="types">The types to subscribe to. leave empty to subscribe to all</param>
        /// <returns>Disposable instance to allow to unsubscribed</returns>
        Task<IDisposable> Subscribe(Action<IEvent> handler, params string[] types);

        /// <summary>
        /// Replays the events.
        /// </summary>
        /// <param name="startEventGuid">Start from this event</param>
        void ReplayEvents(Guid? startEventGuid);

        /// <summary>
        /// Replays the events.
        /// </summary>
        /// <param name="types">The event types.</param>
        /// <param name="startEventGuid">Start from this event</param>
        void ReplayEvents(string[] types, Guid? startEventGuid);
    }
}
