using Optional;
using MOP.Core.Domain.Events;
using System;
using System.Collections.Generic;

namespace MOP.Host.Events
{
    interface IEventStorage
    {
        /// <summary>
        /// Writes the event.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="event">The event.</param>
        void WriteEvent<T>(IEvent<T> @event);
        /// <summary>
        /// Gets the event.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Option<IEvent<object>> GetEvent(Guid id);
        /// <summary>
        /// Gets the events since the event id startId,
        /// if id is not found, a empty list is returned
        /// </summary>
        /// <param name="startId">The start identifier.</param>
        /// <returns></returns>
        IEnumerable<IEvent<object>> GetEvents(Guid startId);
        /// <summary>
        /// Gets the events.
        /// </summary since a date
        /// <param name="from">From.</param>
        /// <returns></returns>
        IEnumerable<IEvent<object>> GetEvents(DateTime from);

        IEnumerable<IEvent<object>> GetAllEvents();
    }
}
