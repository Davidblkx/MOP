using MOP.Core.Akka.Hocon;
using MOP.Terminal.Logger;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MOP.Terminal.Settings
{
    internal class LocalSettings : ISettings
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string? DefaultHost { get ; set; }

        public List<HostSettings> Hosts { get; set; }
            = new List<HostSettings>();

        public bool LogToFile { get; set; }

        public int LogLevel { get; set; } = 2;

        public HoconConfig ActorSystem { get; set; } = new HoconConfig();


        public static LocalSettings FromInterface(ISettings s)
        {
            if (s is LocalSettings local)
                return local;
            return new LocalSettings
            {
                Id = s.Id,
                DefaultHost = s.DefaultHost,
                Hosts = new List<HostSettings>(s.Hosts),
                LogLevel = s.LogLevel,
                LogToFile = s.LogToFile,
                ActorSystem = s.ActorSystem,
            };
        }


        public static ISettings Current { get; private set; } = new LocalSettings();
        public static bool HasInit { get; private set; } = false;

        public static async Task ReloadSettings()
        {
            Current = await SettingsHandler.Instance.Load();
            GlobalLogger.InitLogger();
            HasInit = true;
        }

        public static Task<bool> SaveSettings()
            => SettingsHandler.Instance.Save(Current);
    }
}
