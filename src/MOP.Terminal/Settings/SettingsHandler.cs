using MOP.Core.Helpers;
using MOP.Core.UserSettings;
using Optional.Unsafe;
using Serilog;
using System.IO;
using System.Threading.Tasks;

namespace MOP.Terminal.Settings
{
    internal class SettingsHandler
    {
        private const string FILE_NAME = ".mop-terminal.json";
        private readonly IUserSettingsLoader<ISettings> _loader;

        public SettingsHandler(FileInfo? file = default, ILogger? logger = default)
        {
            _loader = BuildSettingsLoader(file, logger);
        }

        public async Task<ISettings> Load()
        {
            var res = await _loader.Load();
            return res.ValueOrFailure();
        }

        public Task<bool> Save(ISettings settings)
        {
            return _loader.Save(settings);
        }

        private IUserSettingsLoader<ISettings> BuildSettingsLoader(
            FileInfo? file = default, ILogger? logger = default)
        {
            FileInfo settingsFile = file ?? GetDefaultFile();
            var factory = UserSettingsFactory
                .Create<ISettings>()
                .SetDefaultValue(new Settings())
                .SetFile(settingsFile);

            if (!(logger is null))
                factory.SetLogger(logger);

            return factory.Build();
        }

        private FileInfo GetDefaultFile()
        {
            return PathHelpers
                .GetUserHome()
                .RelativeDirectory("MOPTerminal")
                .RelativeFile(FILE_NAME);
        }

        public static SettingsHandler Instance { get; private set; } = new SettingsHandler();
        public static void ReloadInstance(FileInfo? file = default, ILogger? logger = default)
            => Instance = new SettingsHandler(file, logger);
    }
}
