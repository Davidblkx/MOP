using System;

namespace MOP.Core.Domain.Events
{
    public class SubscribeCommand
    {
        public Action<IEvent> Handler { get; }
        public string[] TargetTypes { get; }

        public SubscribeCommand(Action<IEvent> handler, params string[] types)
        {
            Handler = handler;
            TargetTypes = types;
        }

        public static SubscribeCommand Create(Action<IEvent> handler, params string[] types)
            => new SubscribeCommand(handler, types);
    }
}
