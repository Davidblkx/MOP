using MOP.Core.Akka.Hocon;
using System;
using System.Collections.Generic;

namespace MOP.Terminal.Models
{
    /// <summary>
    /// Settings for terminal
    /// </summary>
    /// <seealso cref="MOP.Terminal.Models.ITerminalSettings" />
    public class AppSettings : ITerminalSettings
    {
        public AppSettings() { }
        public AppSettings(ITerminalSettings settings)
        { FromInterface(settings); }

        public Guid Id { get; set; }
        public bool LogToFile { get; set; }
        public int LogLevel { get; set; }
        public string? DefaultHost { get; set; }
        public List<HostConfig> Hosts { get; set; } = new();
        public HoconConfig ActorSystem { get; set; } = new();

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
