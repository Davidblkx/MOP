using Akka.Actor;
using Akka.Configuration;
using MOP.Core.Akka.Hocon;
using MOP.Host.Domain;

namespace MOP.Host.Services
{
    public class ActorSystemFactory
    {
        private readonly HoconConfig _config;

        public ActorSystemFactory(HostProperties props)
        {
            _config = props.ActorSystemConfig;
        }

        public ActorSystem Build()
        {
            var factory = new HoconConfigFactory(_config);
            var configString = factory.Build();
            var actorConfig = ConfigurationFactory.ParseString(configString);
            return ActorSystem.Create("host", actorConfig);
        }

        public static ActorSystem BuildFrom(HostProperties props)
            => new ActorSystemFactory(props).Build();
    }
}
