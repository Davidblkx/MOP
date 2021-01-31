using Akka.Actor;
using MOP.Core.Domain.RIP;
using MOP.Terminal.Logger;
using MOP.Terminal.Settings;
using Serilog;
using MOP.Core.Domain.RIP.Messaging;
using System.Threading.Tasks;
using System;

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
            Receive<Response>(OnResponse);
            Receive<ICall>(OnInvokeCall);
            Receive<object>(OnAny);
        }

        private void OnString(string message)
        {
            _log.Information($"server says: {message}");
        }

        private void OnAny(object? message)
        {
            _log.Information($"server says: {message?.GetType()}");
        }

        private void OnResponse(Response res)
        {
            if (!res.Valid)
            {
                _log.Warning("[@{Code}] Error: {@Message}", res.Error?.code, res.Error?.message);
                return;
            } else if (res.Body is string str)
            {
                Console.WriteLine(str);
                return;
            }

            _log.Warning("Can't read response");
        }

        private void OnInvokeCall(ICall call)
            => AwaitResponse(_server.Ask<Response>(call));

        private async void AwaitResponse(Task<Response> response)
            => OnResponse(await response);

        public static Props BuildProps(IHostSettings host, params string[] remotePaths)
            => Props.Create<TerminalActor>(host, remotePaths);
    }
}
