using Optional;
using System.IO;
using System.Threading.Tasks;

namespace MOP.Infra.UserSettings
{
    /// <summary>
    /// Save and load settings from a JSON file
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IUserSettingsLoader<T>
    {
        /// <summary>
        /// Gets the settings file.
        /// </summary>
        /// <value>
        /// The settings file.
        /// </value>
        public FileInfo SettingsFile { get; }

        /// <summary>
        /// Reads the settings file and parse the JSON value.
        /// Returns the default value if not found
        /// </summary>
        /// <returns></returns>
        public Task<Option<T>> Load();
        /// <summary>
        /// Saves the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns></returns>
        public Task<bool> Save(T settings);
    }
}