using Optional;
using System;

using static MOP.Core.Helpers.NullHelper;

namespace MOP.Core.Domain.Events
{
    public class Event : IEvent
    {
        public Guid Id { get; private set; }
        public string Type { get; }
        public DateTime DateTime { get; private set; }

        public Event(string type)
        {
            Type = type;
            Id = Guid.NewGuid();
            DateTime = DateTime.UtcNow;
        }

        protected Event(string type, DateTime dateTime, Guid id)
        {
            Type = type;
            Id = id;
            DateTime = dateTime;
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

        public static Event Clone(IEvent e)
            => new Event(e.Type, e.DateTime, e.Id);
    }

    public class Event<T> : Event, IEvent<T>
    {
        public Option<T> Body { get; private set; }

        public Event(string type) : base(type) { }
        public Event(string type, T body): base(type)
        {
            Body = Some(body);
        }
        public Event(IEvent e):
            base(e.Type, e.DateTime, e.Id) { }

        public Event(IEvent e, T body) :
            base(e.Type, e.DateTime, e.Id)
        { Body = Some(body); }

        public Event<K> Cast<K>()
        {
            var @new = new Event<K>(this);
            Body.MatchSome(v =>
            {
                if (v is K castValue)
                    @new.Body = Some(castValue);
            });
            return @new;
        }
    }
}
