using MOP.Terminal.CommandLine.Commands;
using MOP.Terminal.Infra;
using MOP.Terminal.Services;
using System;
using System.CommandLine.Facilitator;
using System.Linq;
using System.Threading.Tasks;

namespace MOP.Terminal.CommandLine.Handlers
{
    [HandlerHost]
    internal class SendCommandHandler
    {
        private readonly IActorService _actorService;
        private readonly ISettingsService _settings;

        public SendCommandHandler(IActorService actorService, ISettingsService settings)
        {
            _actorService = actorService;
            _settings = settings;
        }

        [Handler(Target = typeof(SendCommand))]
        public int InvokeCommand(SendCommand o)
        {
            var name = o.HostName ?? _settings.DefaultHost;
            if (name is null)
            {
                Console.WriteLine("Default host is not defined".Error());
                return 1;
            }

            var host = _settings.Hosts.FirstOrDefault(e => e.Name == name);
            if (host is null)
            {
                Console.WriteLine($"Can't find host: {name}".Error());
                return 1;
            }
            return 0;
        }
    }
}
