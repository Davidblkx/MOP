using Akka.Actor;
using Akka.Cluster;
using MOP.Core.Infra.Extensions;
using MOP.Terminal.Actors;
using MOP.Terminal.Factories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOP.Terminal.Services
{
    internal class ActorService : IActorService
    {
        private readonly ISettingsService _settings;

        public ActorSystem ActorSystem { get; }
        public Cluster Cluster { get; }

        public ActorService(ISettingsService settings)
        {
            _settings = settings;
            ActorSystem = new ActorSystemFactory(_settings).Build();
            Cluster = Cluster.Get(ActorSystem);
        }

        public void Start(string[] args)
        {
            var supervisor = ActorSystem
                .ActorOf<SupervisorActor>(SupervisorActor.ACTOR_NAME);
            supervisor.Tell(args);
        }

        public async Task Shutdown()
            => await CoordinatedShutdown
                .Get(ActorSystem)
                .Run(CoordinatedShutdown.ActorSystemTerminateReason.Instance);

        public void Ping(object? message, IActorRef? sender = null)
        {
            var actor = sender ?? ActorRefs.NoSender;
            var hosts = Cluster.State.Members.Where(e => e.HasRole("host"));
            foreach (var h in hosts)
                ActorSystem.ActorSelection($"{h.Address}/user/ping").Tell(message);
        }

        private static string BuildArgument(IEnumerable<string>? p)
            => p is null ? "[]" : $"[{string.Join(',', p.Select(e => ToJsonValue(e)))}]";

        private static string ToJsonValue(string value)
        {
            if (value.IsNullOrEmpty())
                return "\"\"";
            if (double.TryParse(value, out var _))
                return value;
            if (long.TryParse(value, out var _))
                return value;
            if (JSON_VALUES.Any(e => e == value))
                return value;
            if (value[0] == '{' || value[0] == '[')
                return value;

            return $"\"{value}\"";
        }

        private static readonly string[] JSON_VALUES = { "undefined", "null", "true", "false" };
    }
}
