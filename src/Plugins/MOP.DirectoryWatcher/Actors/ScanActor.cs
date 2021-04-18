using Akka.Actor;
using MOP.Core.Domain.Events;
using MOP.Core.Services;
using MOP.DirectoryWatcher.Models;
using Serilog;

namespace MOP.DirectoryWatcher.Actors
{
    public class ScanActor : ReceiveActor
    {
        private readonly ILogger _log;
        private readonly IEventService _eventService;

        public ScanActor(IInjectorService injector)
        {
            _log = injector.GetService<ILogService>()
                .GetContextLogger<ScanActor>();
            _eventService = injector.GetService<IEventService>();

            Receive<ScanDirectory>(OnScanDirectory);
        }

        private void OnScanDirectory(ScanDirectory message)
        {
            var dir = message.Directory;
            var result = new ScanDirectoryResults
                { TaskId = message.TaskId };

            if (dir is null || !dir.Exists)
            {
                Context.Parent.Tell(result);
                return;
            }

            _log.Debug("Scanning directory: {@Directory}", dir.FullName);

            var dirList = dir.GetDirectories();
            var files = dir.GetFiles();
            result.Diretories = dirList.Length;

            foreach (var f in files)
                if (f.Exists)
                    _eventService.Emit(EventTypes.IO.DirectoryFileFound, f);

            _log.Debug("Scanning directory: {@Directory}, found {@Files} files", dir.FullName, files.Length);

            foreach (var d in dirList)
                if (d.Exists)
                    Context.Parent.Tell(new ScanDirectory
                    {
                        Directory = d,
                        TaskId = message.TaskId
                    });

            Context.Parent.Tell(result);
        }
    }
}
