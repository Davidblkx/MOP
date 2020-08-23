using Akka.Actor;
using Akka.Configuration;
using MOP.Core.Akka.Hocon;
using MOP.Host.Domain;
using System;

namespace MOP.Host.Services
{
    public class ActorSystemFactory
    {
        private readonly HoconConfig _config;
        private readonly Guid _id;

        public ActorSystemFactory(HostProperties props)
        {
            _config = props.ActorSystemConfig;
            _id = props.Id;
        }

        public ActorSystem Build()
        {
            var factory = new HoconConfigFactory(_config);
            var configString = factory.Build();
            var actorConfig = ConfigurationFactory.ParseString(configString);
            return ActorSystem.Create(_id.ToString(), actorConfig);
        }

        public static ActorSystem BuildFrom(HostProperties props)
            => new ActorSystemFactory(props).Build();
    }
}
