using MOP.Core.Domain.Events;
using MOP.Core.Services;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using MOP.Core.Infra.Collections;

namespace MOP.Host.Events
{
    /// <summary>
    /// Handles events subscriptions
    /// </summary>
    internal class EventSubscriptionHandler
    {
        private readonly IncrementalDictionary<string> _eventTypes;
        private readonly Dictionary<Guid, IEventSubscription> _subs;
        private readonly IEventStorage _storage;
        private readonly ILogger _log;

        public EventSubscriptionHandler(IEventStorage storage, ILogService logService)
        {
            // Case insensitive dictionary
            _eventTypes = CreateEventDict();
            _subs = new Dictionary<Guid, IEventSubscription>();
            _storage = storage;
            _log = logService.GetContextLogger<EventSubscriptionHandler>();
        }

        /// <summary>
        /// Emits the specified event.
        /// </summary>
        /// <param name="e">The event.</param>
        public void Emit(IEvent<object> e)
        {
            _storage.WriteEvent(e);
            EmitWithoutSave(e);
        }

        /// <summary>
        /// Emits the specified created since the event id.
        /// </summary>
        /// <param name="eventId">The event identifier.</param>
        public void Emit(Guid? eventId, string[] types)
        {
            var events = eventId is Guid id
                ? GetEvents(id, types) : GetEvents(types);
            EmitWithoutSave(events);
        }

        /// <summary>
        /// Emits the events after the specified date time.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        public void Emit(DateTime dateTime, string[] types)
            => EmitWithoutSave(GetEvents(dateTime, types));

        /// <summary>
        /// Subscribes the specified command.
        /// </summary>
        /// <param name="cmd">The command.</param>
        /// <returns></returns>
        public IDisposable Subscribe(SubscribeCommand cmd)
        {
            var sub = CreateSubscription(cmd.Handler, GetEventsId(cmd.TargetTypes));
            _log.Debug("New subscription for types {@TargetTypes} with id: {@Id} ", cmd.TargetTypes, sub.Id);
            return sub;
        }

        /// <summary>
        /// Gets the events identifier.
        /// </summary>
        /// <param name="events">The events.</param>
        /// <returns></returns>
        private IEnumerable<ulong> GetEventsId(string[] events)
            => events.Select(t => _eventTypes.GetId(t));

        /// <summary>
        /// Creates the subscription.
        /// </summary>
        /// <param name="cmd">The command.</param>
        /// <returns></returns>
        private IEventSubscription CreateSubscription(Action<IEvent> handler, IEnumerable<ulong> typesId)
        {
            // Create the subscription
            var sub = new EventSubscription(handler, typesId);
            // Add new subscription
            _subs.Add(sub.Id, sub);
            // Remove subscription on dispose
            sub.OnDispose += (_, uid) =>
            {
                _subs.Remove(uid);
                _log.Debug("Unsubscribed id {@Uid}", uid);
            };

            return sub;
        }

        /// <summary>
        /// Emits all events without save.
        /// </summary>
        /// <param name="events">The events.</param>
        private void EmitWithoutSave(IEnumerable<IEvent<object>> events)
        {
            foreach (var e in events)
                EmitWithoutSave(e);
        }

        /// <summary>
        /// Emits a event without save.
        /// </summary>
        /// <param name="e">The e.</param>
        private void EmitWithoutSave(IEvent<object> e)
        {
            var typeId = _eventTypes.GetId(e.Type);
            _subs.Values
                .Where(s => 
                    s.EventTypes.Contains(typeId) 
                    || s.EventTypes.Count() == 0
                ).ToList().ForEach(s => s.Handler(e));
        }

        private IEnumerable<IEvent<object>> GetEvents(string[] types)
            => FilterByType(_storage.GetAllEvents(), types);

        private IEnumerable<IEvent<object>> GetEvents(Guid id, string[] types)
            => FilterByType(_storage.GetEvents(id), types);

        private IEnumerable<IEvent<object>> GetEvents(DateTime date, string[] types)
            => FilterByType(_storage.GetEvents(date), types);

        private IEnumerable<IEvent<object>> FilterByType(IEnumerable<IEvent<object>> events, string[] types)
            => types.Length == 0 ? events : events.Where(e => types.Contains(e.Type));

        /// <summary>
        /// Creates the event dictionary and add a entry for all_events.
        /// </summary>
        /// <returns></returns>
        private IncrementalDictionary<string> CreateEventDict()
                => IncrementalDictionary.Create<string>(e => e.ToUpper());
    }
}
