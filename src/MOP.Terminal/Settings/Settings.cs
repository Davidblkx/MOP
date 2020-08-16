using System;
using System.Collections.Generic;

namespace MOP.Terminal.Settings
{
    internal class Settings : ISettings
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public int Port { get; set; }

        public string Hostname { get; set; } = "localhost";

        public string? DefaultHost { get ; set; }

        public IEnumerable<IHostSettings> Hosts { get; set; }
            = new List<HostSettings>();

        public bool LogToFile { get; set; }
    }
}
