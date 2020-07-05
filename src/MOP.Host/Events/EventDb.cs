using Akka.Util;
using MOP.Core.Domain.Events;
using System;
using static MOP.Core.Helpers.NullHelper;

namespace MOP.Host.Events
{
    /// <summary>
    /// Event to be stored in database
    /// </summary>
    internal class EventDb : IEvent
    {
        public Guid Id { get; set; }
        public string Type { get; set; } = "";
        public DateTime DateTime { get; set; }
        public object? Body { get; set; }

        public EventDb() { }

        public IEvent<object> ToEvent()
        {
            Option<object> obj = Body is null
                ? None<object>()
                : Some(Body);
            return new Event<object>(this, obj);
        }

        public bool Equals(IEvent x, IEvent y)
           => x.Id.Equals(y.Id);

        public int GetHashCode(IEvent obj)
            => obj.Id.GetHashCode();

        public int CompareTo(object obj)
        {
            if (obj is IEvent e)
                return CompareTo(e);
            throw new ArgumentException("Object is not a IEvent");
        }

        public int CompareTo(IEvent other)
            => Id.CompareTo(other.Id);

        public bool Equals(IEvent other)
            => Id.Equals(other.Id);

        public static EventDb From<T>(IEvent<T> e)
            => new EventDb
            {
                Id = e.Id,
                Type = e.Type,
                DateTime = e.DateTime,
                Body = e.Body.ValueOr(null)
            };
    }
}
