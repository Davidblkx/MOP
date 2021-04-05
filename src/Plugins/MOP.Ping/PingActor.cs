using Akka.Actor;
using MOP.Core.Services;
using Serilog;

namespace MOP.Ping
{
    public class PingActor : ReceiveActor
    {
        private readonly ILogger _log;

        public PingActor(ILogService log)
        {
            _log = log.GetContextLogger<PingActor>();
            ReceiveAny(OnAny);
        }

        public void OnAny(object? message)
        {
            if (Sender != ActorRefs.Nobody)
            {
                _log.Information($"Receive ping from {Sender.Path}");
                Sender.Tell(message);
            }
        }
    }
}
