using MOP.Host.Domain;
using MOP.Infra.UserSettings;
using Optional.Unsafe;
using System.IO;
using System.Threading.Tasks;

namespace MOP.Host.Factories
{
    public class HostPropertiesFactory
    {
        private readonly IUserSettingsLoader<HostProperties> _loader;

        public HostPropertiesFactory(string[] args)
        {
            var file = PropertiesFileFactory.BuildPropertiesFile(args);
            _loader = BuildLoader(file);
        }

        public async Task<HostProperties> Build()
            => (await _loader.Load()).ValueOrFailure();

        private IUserSettingsLoader<HostProperties> BuildLoader(FileInfo file)
        {
            var factory = UserSettingsFactory
                .Create<HostProperties>()
                .SetDefaultValue(new HostProperties())
                .SetFile(file);

            return factory.Build();
        }

        public static Task<HostProperties> BuildPropertiesAsync(string[] args)
            => new HostPropertiesFactory(args).Build();
    }
}
