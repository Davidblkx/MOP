using MOP.Terminal.CommandLine;
using MOP.Terminal.Settings;
using System.CommandLine.Builder;
using System.Threading.Tasks;
using System;
using System.Linq;
using MOP.Terminal.ActorsSystem;
using System.CommandLine;
using System.CommandLine.Invocation;
using MOP.Core.Infra.Extensions;
using MOP.Core.Domain.RIP.Messaging;

namespace MOP.Terminal.CommandLineTransformers
{
    public class SendCommand : ICommandBuilderFactory
    {
        public Task<CommandLineBuilder> Transform(CommandLineBuilder builder)
        {
            return Task.Run(() => builder.AddCommand(CreateSendCommand()));
        }

        private Command CreateSendCommand()
        {
            var cmd = new Command("send", "send a message to a host");
            cmd.AddArgument(new Argument<string>("command", "command to load") { Arity = ArgumentArity.ExactlyOne });
            cmd.AddArgument(new Argument<string>("action", "action to invoke") { Arity = ArgumentArity.ExactlyOne });
            cmd.Handler = CommandHandler.Create((string command, string action, string host) => OnSend(command, action, host));
            return cmd;
        }

        private void OnSend(string command, string action, string host)
        {
            var hostSettings = GetHost(GetHostName(host));
            var actor = LocalActorSystem.Ref.CreateTerminalActor(hostSettings, "RIP");
            actor.Tell(new RemoteCall(command, action), actor);
        }

        private IHostSettings GetHost(string name)
        {
            var res = LocalSettings.Current
                .Hosts.FirstOrDefault(e => e.Name.EqualIgnoreCase(name));
            if (res is null)
                throw new ArgumentNullException("Can't find host with name " + name);
            return res;
        }

        private string GetHostName(string name)
        {
            string? host = name.IsNullOrEmpty() ?
                LocalSettings.Current.DefaultHost : name;

            if (host.IsNullOrEmpty())
                throw new ArgumentException("Default host is not set");

            return host;
        }
    }
}
