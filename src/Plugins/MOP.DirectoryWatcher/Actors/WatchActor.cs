using Akka.Actor;
using MOP.Core.Services;
using MOP.DirectoryWatcher.Models;
using Serilog;
using System.IO;

using static MOP.Core.Domain.Events.EventTypes.IO;

namespace MOP.DirectoryWatcher.Actors
{
    public class WatchActor : ReceiveActor
    {
        private readonly ILogger _log;
        private readonly IEventService _eventService;
        private readonly FolderToWatch _folder;

        public WatchActor(IInjectorService injector, FolderToWatch folder)
        {
            _log = injector.GetService<ILogService>()
                .GetContextLogger<WatchActor>();
            _eventService = injector.GetService<IEventService>();
            _folder = folder;

            StartWatcher(folder.FolderPath);
        }

        private void StartWatcher(string path)
        {
            var watcher = new FileSystemWatcher(path)
            {
                IncludeSubdirectories = true,
                NotifyFilter = NotifyFilters.LastWrite
                    | NotifyFilters.Size
                    | NotifyFilters.FileName
                    | NotifyFilters.DirectoryName
                    | NotifyFilters.Size
                    | NotifyFilters.CreationTime,

                EnableRaisingEvents = true,
                Filter = "*.*",
            };

            watcher.Changed += (_, e) => OnFileChange(e);
            watcher.Created += (_, e) => OnFileChange(e);
            watcher.Deleted += (_, e) => OnFileChange(e);
            watcher.Renamed += (_, e) => OnFileChange(e);
            watcher.Error += (_, e) => OnError(e);
        }

        private void OnFileChange(FileSystemEventArgs e)
        {
            var type = e.ChangeType switch
            {
                WatcherChangeTypes.All => DirectoryFileChanged,
                WatcherChangeTypes.Created => DirectoryFileAdded,
                WatcherChangeTypes.Changed => DirectoryFileChanged,
                WatcherChangeTypes.Deleted => DirectoryFileRemoved,
                WatcherChangeTypes.Renamed => DirectoryFileMoved,
                _ => null,
            };

            if (type is null) return;
            _eventService.Emit(type, e);
        }

        private void OnError(ErrorEventArgs e)
            => _log.Error("{@Path}: {@Error}", _folder.FolderPath, e.GetException().Message);
    }
}
