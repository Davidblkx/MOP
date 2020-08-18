using Akka.Util;
using MOP.Infra.Domain.Events;
using System;
using System.Runtime.CompilerServices;
using static MOP.Infra.Optional.Static;

[assembly: InternalsVisibleTo("MOP.Host.Test")]
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
            return Body is null
                ? new Event<object>(this)
                : new Event<object>(this, Body);
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
#pragma warning disable CS8604 // Possible null reference argument.
                Body = e.Body.ValueOr(default(T))
#pragma warning restore CS8604 // Possible null reference argument.
            };
    }
}
