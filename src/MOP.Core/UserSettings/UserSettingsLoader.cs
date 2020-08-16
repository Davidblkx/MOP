using Newtonsoft.Json;
using Optional;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;

using static MOP.Core.Optional.StaticOption;

namespace MOP.Core.UserSettings
{
    internal class UserSettingsLoader<T> : IUserSettingsLoader<T>
    {
        private readonly ILogger _log;
        private readonly T _default;
        public FileInfo SettingsFile { get; }

        public UserSettingsLoader(FileInfo file, ILogger logger, T defaultValue)
        {
            SettingsFile = file;
            _log = logger;
            _default = defaultValue;
        }

        public async Task<Option<T>> Load()
        {
            try
            {
                if (!SettingsFile.Exists)
                    return await InitSettings();

                var json = await File.ReadAllTextAsync(SettingsFile.FullName);
                return Some(JsonConvert.DeserializeObject<T>(json));
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Error loading settings file: {FullName}", SettingsFile.FullName);
                return None<T>();
            }
        }

        public async Task<bool> Save(T settings)
        {
            try
            {
                if (settings is null)
                {
                    _log.Warning("Invalid settings object: null");
                    return false;
                }

                await TrySave(settings);
                return true;
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Error saving settings file: {FullName}", SettingsFile.FullName);
                return false;
            }
        }

        private async Task TrySave(T settings)
        {
            if (!SettingsFile.Directory.Exists)
                SettingsFile.Directory.Create();

            var json = JsonConvert.SerializeObject(settings);
            await File.WriteAllTextAsync(SettingsFile.FullName, json);
        }

        /// <summary>
        /// Initializes the settings.
        /// </summary>
        /// <returns></returns>
        private async Task<Option<T>> InitSettings()
        {
            if (await Save(_default))
                return Some(_default);
            return None<T>();
        }
    }
}
