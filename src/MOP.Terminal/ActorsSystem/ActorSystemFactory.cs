using Akka.Actor;
using Akka.Configuration;
using MOP.Core.Infra.AKKA;
using MOP.Terminal.Settings;
using System;

namespace MOP.Terminal.ActorsSystem
{
    public class ActorSystemFactory
    {
        private readonly HoconConfigFactory _hoconFactory;
        private Guid? _id;
        
        private const string CONFIG_PORT = "PORT";
        private const string CONFIG_HOSTNAME = "HOSTNAME";

        public ActorSystemFactory()
        {
            _hoconFactory = new HoconConfigFactory(TerminalHoconConfig.CONFIG);
        }

        public ActorSystemFactory SetSettings(ISettings s)
        {
            _hoconFactory.Set(CONFIG_PORT, s.Port.ToString());
            _hoconFactory.Set(CONFIG_HOSTNAME, s.Hostname);
            _id = s.Id;
            return this;
        }

        public ActorSystem Build()
        {
            if (_id is null)
                throw new ArgumentNullException("Settings must be applied before building the system");

            var config = ConfigurationFactory.ParseString(_hoconFactory.Build());
            return ActorSystem.Create(_id.ToString(), config);
        }

        public static ActorSystem Build(ISettings settings)
            => new ActorSystemFactory()
                .SetSettings(settings)
                .Build();
    }
}
