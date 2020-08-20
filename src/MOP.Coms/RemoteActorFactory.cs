using Akka.Actor;
using MOP.Infra.Domain.Actors;
using MOP.Infra.Domain.Host;
using MOP.Infra.Services;
using Optional;
using Serilog;
using System;

using static MOP.Core.Infra.Optional.Static;

namespace MOP.Remote
{
    internal class RemoteActorFactory : IActorFactory
    {
        public string ActorRefName => "MopRemote";

        public IActorRefInstanceType InstanceType => IActorRefInstanceType.Singleton;

        private readonly ILogService _service;
        private readonly ILogger _logs;
        private readonly IHostInfo _info;

        public RemoteActorFactory(ILogService service, IHostInfo info)
        {
            _info = info;
            _service = service;
            _logs = service.GetContextLogger<RemoteActorFactory>();
        }

        public Option<IActorRef> BuildActorRef(ActorSystem actorSystem)
        {
            try
            {
                var props = Props.Create<MopRemoteActor>(_service, _info);
                return Some(actorSystem.ActorOf(props, ActorRefName));
            }
            catch (Exception ex)
            {
                _logs.Error(ex, "Error creating MopRemote actor");
                return None<IActorRef>();
            }
        }
    }
}
