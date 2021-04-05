using Akka.Actor;
using Akka.Configuration;
using MOP.Core.Akka.Hocon;
using MOP.Terminal.Services;

namespace MOP.Terminal.Factories
{
    /// <summary>
    /// Builds the actor system
    /// </summary>
    internal class ActorSystemFactory
    {
        private readonly ISettingsService _settings;

        public ActorSystemFactory(ISettingsService settings)
        {
            _settings = settings;
        }

        public ActorSystem Build()
        {
            _settings.ActorSystem.EnsureRole("terminal");
            var configString = new HoconConfigFactory(_settings.ActorSystem).Build();
            var actorConfig = ConfigurationFactory.ParseString(configString);
            return ActorSystem.Create(_settings.ActorSystem.ActorSystemName, actorConfig);
        }
    }
}
