using Akka.Actor;
using Akka.Routing;
using MOP.Core.Domain.Events.Progress;
using MOP.Core.Services;
using MOP.DirectoryWatcher.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;

namespace MOP.DirectoryWatcher.Actors
{
    /// <summary>
    /// Track scan jobs and report progress events
    /// </summary>
    public class ScanSupervisor : ReceiveActor
    {
        private readonly IActorRef _scanActors;
        private readonly IEventService _events;
        private readonly ILogger _log;

        private readonly Dictionary<Guid, TaskReport> _tasks = new();

        public ScanSupervisor(int concurrent, IInjectorService injector)
        {
            _events = injector.GetService<IEventService>();
            _log = injector.GetService<ILogService>()
                .GetContextLogger<ScanSupervisor>();

            _scanActors = Context.ActorOf(
                Props.Create<ScanActor>(injector)
                    .WithRouter(new RoundRobinPool(concurrent))
            );

            Receive<DirectoryInfo>(OnDirectory);
            Receive<ScanDirectory>(e => _scanActors.Tell(e));
            Receive<ScanDirectoryResults>(OnProgress);
            Receive<Guid>(OnGuid);
        }

        /// <summary>
        /// Start a new task tracker for a directory
        /// </summary>
        /// <param name="info"></param>
        private void OnDirectory(DirectoryInfo info)
        {
            if (!info.Exists) return;

            _log.Debug("Start scanning directory: {@Directory}", info.FullName);

            var taskStart = new TaskStarted();
            var id = taskStart.TaskID;
            var report = new TaskReport(id, 0, 1);
            _tasks.Add(id, report);
            _events.Emit(taskStart);

            _scanActors.Tell(new ScanDirectory
            {
                Directory = info,
                TaskId = id
            });
        }

        private void OnProgress(ScanDirectoryResults results)
        {
            if (!_tasks.ContainsKey(results.TaskId)) return;

            var task = _tasks[results.TaskId].Increment(1, results.Diretories);

            _events.Emit(new TaskProgressReport(task));
            _tasks[task.TaskId] = task;

            Self.Tell(task.TaskId);
        }

        private void OnGuid(Guid id)
        {
            if (!_tasks.ContainsKey(id)) return;

            var task = _tasks[id];

            if (task.ProgressCount < task.ProgressTotal) return;

            _tasks.Remove(id);
            _events.Emit(new TaskEnd(id));
        }
    }
}
