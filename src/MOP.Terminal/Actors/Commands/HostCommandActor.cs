using MOP.Terminal.CommandLine.Commands;
using System.Threading.Tasks;
using MOP.Terminal.Services;
using MOP.Terminal.Infra;
using ConsoleInteractive;

using static MOP.Terminal.Services.HostsHandlerService;
using static MOP.Terminal.Infra.DependencyInjector;

namespace MOP.Terminal.Actors
{
    [CommandActor(HostCommandList.NAME)]
    internal class HostCommandActorList : InternalBaseCommandActor<HostCommandList>
    {
        protected override Task<int> HandleCommand(HostCommandList command)
        {
            foreach (var h in GetInstance<IHostsHandlerService>().GetHostConfigs())
                WriteLine(FormatHostConfigOutput(h));

            return Task.FromResult(0);
        }
    }

    [CommandActor(HostCommandSet.NAME)]
    internal class HostCommandActorSet : InternalBaseCommandActor<HostCommandSet>
    {
        protected override async Task<int> HandleCommand(HostCommandSet o)
        {
            var _handler = GetInstance<IHostsHandlerService>();
            if (_handler.HasHost(o.Name) && !o.Replace)
            {
                var message = $"Name [{o.Name} already exists, use --replace to force save]";
                WriteLine(message.Error());
                return 1;
            }

            await _handler.SetHost(o, o.IsDefault);
            WriteLine("Host saved!".Success());
            return 0;
        }
    }

    [CommandActor(HostCommandRemove.NAME)]
    internal class HostCommandActorRemove : InternalBaseCommandActor<HostCommandRemove>
    {
        protected override async Task<int> HandleCommand(HostCommandRemove o)
        {
            var _handler = GetInstance<IHostsHandlerService>();
            if (!_handler.HasHost(o.Name))
            {
                WriteLine($"Can't find host: {o.Name}".Error());
                return 1;
            }

            var message = $"Do you want to remove host {o.Name}".Warn();
            if (o.Force || ConsoleI.AskConfirmation(message))
            {
                await _handler.RemoveHost(o.Name);
                WriteLine("Host removed".Success());
            }
            return 0;
        }
    }
}
