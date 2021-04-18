using System;

namespace MOP.Core.Domain.Events.Progress
{
    public class TaskReport
    {
        public Guid TaskId { get; set; }
        public long ProgressCount { get; set; }
        public long ProgressTotal { get; set; }
        public bool IsIndeterminated => ProgressTotal == 0;

        public TaskReport() { }
        public TaskReport(Guid taskId, long count, long total = 0)
        {
            TaskId = taskId;
            ProgressCount = count;
            ProgressTotal = total;
        }

        public TaskReport Increment(int byCount, int byTotal = 0)
            => new(TaskId, ProgressCount + byCount, ProgressTotal + byTotal);
    }
}
