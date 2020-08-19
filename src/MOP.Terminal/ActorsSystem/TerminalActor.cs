using Akka.Actor;
using MOP.Terminal.Logger;
using MOP.Terminal.Settings;
using Serilog;

namespace MOP.Terminal.ActorsSystem
{
    public class TerminalActor: ReceiveActor
    {
        private readonly ILogger _log;
        private readonly ActorSelection _server;

        public TerminalActor(IHostSettings host, params string[] remotePaths)
        {
            _log = GlobalLogger.ForContext<TerminalActor>();
            _server = Context.ActorSelection(AkkaAddressFactory.BuildTcp(host, remotePaths));
            ConfigureEndpoints();
        }

        private void ConfigureEndpoints()
        {
            Receive<string>(OnString);
            Receive<ServerMessage>(OnServerMessage);
        }

        private void OnString(string message)
        {
            _log.Information($"server says: {message}");
        }

        private void OnServerMessage(ServerMessage message)
        {
            _server.Tell(message.Message);
        }

        public static Props BuildProps(IHostSettings host, params string[] remotePaths)
            => Props.Create<TerminalActor>(host, remotePaths);
    }
}
