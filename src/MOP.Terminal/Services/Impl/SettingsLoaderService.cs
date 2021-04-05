using MOP.Core.Infra.Extensions;
using MOP.Core.Infra.Tools;
using MOP.Core.UserSettings;
using MOP.Terminal.Models;
using Optional.Unsafe;
using System.IO;
using System.Threading.Tasks;

namespace MOP.Terminal.Services
{
    /// <summary>
    /// Load the settings file, if does not exists, a new one is created
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SettingsLoaderService<T> : ISettingsLoaderService<T> where T : new()
    {
        // Default file name
        private const string FILE_NAME = ".mop-terminal.json";
        // File to save to
        private readonly FileInfo _file;
        // Settings saver/loader
        private readonly IUserSettingsLoader<T> _settingsLoader;

        public SettingsLoaderService(StartupArgs args)
        {
            // Select the file to use
            _file = string.IsNullOrEmpty(args.SettingsFile)
                ? GetDefaultFile()
                : new FileInfo(args.SettingsFile);

            // Init loader
            _settingsLoader = UserSettingsFactory
                .Create<T>()
                .SetDefaultValue(new T())
                .SetFile(_file)
                .Build();
        }

        /// <summary>
        /// Loads this settings instance.
        /// </summary>
        /// <returns></returns>
        public async Task<T> Load()
            => (await _settingsLoader.Load()).ValueOrFailure();

        /// <summary>
        /// Saves the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>true, if success</returns>
        public Task<bool> Save(T value)
            => _settingsLoader.Save(value);

        /// <summary>
        /// Gets the default file path.
        /// </summary>
        /// <returns></returns>
        private static FileInfo GetDefaultFile()
        {
            return PathTools
                .GetUserHome()
                .RelativeDirectory("MOPTerminal")
                .RelativeFile(FILE_NAME);
        }
    }
}
