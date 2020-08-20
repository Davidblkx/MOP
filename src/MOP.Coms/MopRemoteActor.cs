using Akka.Actor;
using MOP.Core.Infra.Extensions;
using MOP.Infra.Domain.Host;
using MOP.Infra.Services;
using Serilog;

namespace MOP.Remote
{
    internal class MopRemoteActor : ReceiveActor
    {
        private readonly ILogger _log;
        private readonly IHostInfo _info;

        public MopRemoteActor(ILogService logService, IHostInfo info)
        {
            _log = logService.GetContextLogger<MopRemoteActor>();
            _info = info;
            Receive<string>(HandleMessage);
        }

        private void HandleMessage(string message)
        {
            if (message.EqualIgnoreCase("info"))
            {
                Sender.Tell("ola");
                return;
            }

            _log.Information($"Remote actor says: {message}");
        }
    }
}
