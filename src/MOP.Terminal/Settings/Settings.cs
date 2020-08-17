using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MOP.Terminal.Settings
{
    internal class LocalSettings : ISettings
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public int Port { get; set; }

        public string Hostname { get; set; } = "localhost";

        public string? DefaultHost { get ; set; }

        public IEnumerable<IHostSettings> Hosts { get; set; }
            = new List<HostSettings>();

        public bool LogToFile { get; set; }

        public static ISettings Current { get; private set; } = new LocalSettings();
        public static async Task ReloadSettings()
            => Current = await SettingsHandler.Instance.Load();
    }
}
