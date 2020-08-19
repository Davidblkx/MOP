using MOP.Terminal.CommandLine;
using MOP.Terminal.Settings;
using MOP.Infra.Extensions;
using System.CommandLine.Builder;
using System.Threading.Tasks;
using System;
using System.Linq;
using MOP.Terminal.ActorsSystem;
using System.CommandLine;
using System.CommandLine.Invocation;

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
            cmd.AddArgument(new Argument<string>("path", "path to target") { Arity = ArgumentArity.ExactlyOne });
            cmd.AddArgument(new Argument<string>("message", "message to send") { Arity = ArgumentArity.ExactlyOne });
            cmd.Handler = CommandHandler.Create((string path, string message, string host) => OnSend(path, message, host));
            return cmd;
        }

        private void OnSend(string path, string message, string host)
        {
            var hostSettings = GetHost(GetHostName(host));
            var actor = LocalActorSystem.Ref.CreateTerminalActor(hostSettings, path);
            actor.Tell(ServerMessage.Create(message), actor);
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
