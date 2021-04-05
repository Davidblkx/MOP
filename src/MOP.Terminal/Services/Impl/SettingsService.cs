using MOP.Core.Akka.Hocon;
using MOP.Terminal.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MOP.Terminal.Services
{
    internal class SettingsService : ISettingsService
    {
        private readonly ISettingsLoaderService<AppSettings> _loader;

        public SettingsService(ISettingsLoaderService<AppSettings> loader)
        {
            _loader = loader;
            FirstLoad();
        }

        public Guid Id { get; set; }
        public bool LogToFile { get; set; }
        public int LogLevel { get; set; }
        public string? DefaultHost { get; set; }
        public List<HostConfig> Hosts { get; set; } = new();
        public HoconConfig ActorSystem { get; set; } = new();

        /// <summary>
        /// Saves the settings asynchronous.
        /// </summary>
        /// <returns></returns>
        public Task<bool> SaveAsync()
            => _loader.Save(new AppSettings(this)) ?? Task.FromResult(false);

        /// <summary>
        /// Loads the settings asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> LoadAsync()
        {
            var s = await _loader.Load();
            FromInterface(s);
            return true;
        }

        private void FirstLoad()
        {
            var s = _loader.Load().Result;
            FromInterface(s);
        }

        private void FromInterface(ITerminalSettings s)
        {
            Id = s.Id;
            LogToFile = s.LogToFile;
            LogLevel = s.LogLevel;
            DefaultHost = s.DefaultHost;
            Hosts = s.Hosts;
            ActorSystem = s.ActorSystem;
        }
    }
}
