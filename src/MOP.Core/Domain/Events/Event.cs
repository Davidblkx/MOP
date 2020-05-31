using Optional;
using System;

namespace MOP.Core.Domain.Events
{
    internal class Event : IEvent
    {
        public Guid Id => Guid.NewGuid();
        public string Type { get; }
        public DateTime DateTime => DateTime.UtcNow;

        public Event(string type)
        {
            Type = type;
        }

        public bool Equals(IEvent x, IEvent y)
            => x.Id.Equals(y);

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
    }

    internal class Event<T> : Event, IEvent<T>
    {
        public Option<T> Body { get; }

        public Event(string type) : base(type) { }
        public Event(string type, T body): base(type)
        {
            Body = Option.Some(body);
        }
    }
}
