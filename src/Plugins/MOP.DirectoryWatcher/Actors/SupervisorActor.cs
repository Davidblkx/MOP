using Akka.Actor;
using MOP.Core.Domain.Events;
using MOP.Core.Services;
using MOP.DirectoryWatcher.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MOP.DirectoryWatcher.Actors
{
    public class SupervisorActor : ReceiveActor
    {
        private readonly IInjectorService _injector;
        private readonly ILogger _log;

        private readonly DirectoryWatchConfig _config;
        private readonly Dictionary<string, IActorRef> _watchActors;

        private readonly IActorRef _configActor;
        private readonly string[] _supportedTypes;

        public SupervisorActor(IInjectorService injector, DirectoryWatchConfig config)
        {
            _injector = injector;
            _config = config;
            _watchActors = new();
            _configActor = CreateConfigActor();
            _supportedTypes = new string[]
            {
                EventTypes.AddWatchDirectory,
                EventTypes.RemoveWatchDirectory
            };
            
            _log = injector.GetService<ILogService>()
                .GetContextLogger<SupervisorActor>();

            foreach (var d in config.Directories)
                StartWatchActor(d);

            Receive<IEvent>(OnEvent);
        }

        public void OnEvent(IEvent @event)
        {
            if (!_supportedTypes.Contains(@event.Type))
            {
                _log.Warning("WatchDirectory can't process events of type {@Type}", @event.Type);
                return;
            }

            if (@event is IEvent<string> e && e.Body.HasValue)
            {
                if (e.Type == EventTypes.AddWatchDirectory)
                    AddDirectory(e.Body.ValueOr(""));
                else if (e.Type == EventTypes.RemoveWatchDirectory)
                    RemoveDirectory(e.Body.ValueOr(""));

                return;
            }

            _log.Warning("WatchDirectory can't process event {@event}", @event);
        }

        private void RemoveDirectory(string path)
        {
            var info = new DirectoryInfo(path);
            if (!_watchActors.ContainsKey(info.FullName))
            {
                _log.Warning("Directory {@Path}, is not in watch list", info.FullName);
                return;
            }

            _log.Information("Removing path: {@Path}", info.FullName);
            _watchActors[info.FullName].Tell(PoisonPill.Instance);
            _config.Directories.RemoveAll(e => e.FolderPath == info.FullName);
            _configActor.Tell(_config);
        }

        private void AddDirectory(string path)
        {
            var info = new DirectoryInfo(path);

            if (!info.Exists)
            {
                _log.Warning("can't access {@Path}", info.FullName);
                return;
            }

            if (_watchActors.ContainsKey(info.FullName))
            {
                _log.Warning("Directory {@Path}, is already in watch list", info.FullName);
                return;
            }

            var toWatch = new FolderToWatch
            {
                Added = DateTime.UtcNow,
                FolderPath = info.FullName
            };

            _log.Information("Adding path: {@Path}", info.FullName);
            _config.Directories.Add(toWatch);
            StartWatchActor(toWatch);
            _configActor.Tell(_config);
        }


        private void StartWatchActor(FolderToWatch folder)
        {
            if (_watchActors.ContainsKey(folder.FolderPath))
                return;

            var actor = Context.ActorOf(
                Props.Create<WatchActor>(_injector, folder));
            _watchActors.Add(folder.FolderPath, actor);
        }

        private IActorRef CreateConfigActor()
            => Context.ActorOf(Props.Create<ConfigActor>(_injector));
    }
}
