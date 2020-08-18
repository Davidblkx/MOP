using Akka.Actor;
using MOP.Infra.Domain.Host;
using MOP.Infra.Extensions;
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
            if (message.EqualIgnoreCase(message))
            {
                Sender.Tell(_info);
                return;
            }

            _log.Information($"Remote actor says: {message}");
        }
    }
}
