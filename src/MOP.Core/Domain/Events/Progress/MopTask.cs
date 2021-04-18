using System;

namespace MOP.Core.Domain.Events.Progress
{
    public class MopTask
    {
        public Guid TaskID { get; set; }
        public bool IsIndeterminate { get; set; }
        public long Current { get; set; }
        public long Total { get; set; }
    }
}
