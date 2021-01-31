using Akka.Actor;
using Akka.Configuration;
using MOP.Core.Akka.Hocon;
using MOP.Terminal.Settings;
using System;

namespace MOP.Terminal.ActorsSystem
{
    public class ActorSystemFactory
    {
        private ISettings? _settings;

        public ActorSystemFactory SetSettings(ISettings s)
        {
            _settings = s;
            return this;
        }

        public ActorSystem Build()
        {
            if (_settings is null)
                throw new ArgumentNullException("Settings must be applied before building the system");

            var hoconFactory = new HoconConfigFactory(_settings);
            var config = ConfigurationFactory.ParseString(hoconFactory.Build());
            return ActorSystem.Create(_settings.Id.ToString(), config);
        }

        public static ActorSystem Build(ISettings settings)
            => new ActorSystemFactory()
                .SetSettings(settings)
                .Build();
    }
}
