using Akka.Actor;
using MOP.Core.Services;
using MOP.DirectoryWatcher.Models;
using Serilog;
using System;
using System.Threading.Tasks;

namespace MOP.DirectoryWatcher.Actors
{
    public class ConfigActor : ReceiveActor
    {
        private readonly ILogger _log;
        private readonly IConfigService _config;

        public ConfigActor(IInjectorService injector)
        {
            _log = injector.GetService<ILogService>()
                .GetContextLogger<ConfigActor>();

            _config = injector.GetService<IConfigService>();
            ReceiveAsync<DirectoryWatchConfig>(OnConfig);
        }

        public async Task OnConfig(DirectoryWatchConfig config)
        {
            _log.Debug($"Saving settings for directory watch plugin");
            await _config.SaveConfig(Guid.Parse(Info.ID), config);
        }
    }
}
