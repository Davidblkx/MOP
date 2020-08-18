
using System;

namespace MOP.Infra.Domain.Events
{
    public class ReplayCommand
    {
        /// <summary>
        /// Gets the start identifier.
        /// </summary>
        /// <value>
        /// The start identifier.
        /// </value>
        public Guid? StartId { get; }

        /// <summary>
        /// Gets the event types to replay, leave empty to replay all.
        /// </summary>
        /// <value>
        /// The event types.
        /// </value>
        public string[] EventTypes { get; }

        public ReplayCommand(string[] eventTypes, Guid? startEventId)
        {
            EventTypes = eventTypes;
            StartId = startEventId;
        }

        public static ReplayCommand Create(Guid? startId = default)
            => new ReplayCommand(new string[0], startId);
        public static ReplayCommand Create(string[] types, Guid? startId = default)
            => new ReplayCommand(types, startId);
    }
}
