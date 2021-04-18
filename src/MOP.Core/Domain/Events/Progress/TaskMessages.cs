using System;
using Optional.Unsafe;

namespace MOP.Core.Domain.Events.Progress
{
    public class TaskStarted : Event<Guid>
    {
        public TaskStarted() : base(
            EventTypes.Progress.TaskStarted,
            Guid.NewGuid()
        ) { }

        public Guid TaskID => Body.ValueOrFailure();
    }

    public class TaskEnd : Event<Guid>
    {
        public TaskEnd(Guid taskId) : base(
            EventTypes.Progress.TaskEnded,
            taskId
        )
        { }
    }

    public class TaskProgressReport : Event<TaskReport>
    {
        public TaskProgressReport(TaskReport report) : base (
            EventTypes.Progress.ProgressReport,
            report
        ) { }

        public static TaskProgressReport Create(Guid taskId, long count)
            => new(new TaskReport(taskId, count));

        public static TaskProgressReport Create(Guid taskId, long count, long total)
            => new(new TaskReport(taskId, count, total));
    }
}
