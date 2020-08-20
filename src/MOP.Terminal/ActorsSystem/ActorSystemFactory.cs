using Akka.Actor;
using Akka.Configuration;
using MOP.Core.Akka.Hocon;
using MOP.Terminal.Settings;
using System;

namespace MOP.Terminal.ActorsSystem
{
    public class ActorSystemFactory
    {
        private readonly HoconConfigFactory _hoconFactory;
        private Guid? _id;

        public ActorSystemFactory()
        {
            _hoconFactory = new HoconConfigFactory(TerminalHoconConfig.CONFIG);
        }

        public ActorSystemFactory SetSettings(ISettings s)
        {
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
