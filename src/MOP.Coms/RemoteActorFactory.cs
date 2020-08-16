using Akka.Actor;
using MOP.Core.Domain.Actors;
using MOP.Core.Domain.Host;
using MOP.Core.Services;
using Optional;
using Serilog;
using System;

using static MOP.Core.Optional.StaticOption;

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
