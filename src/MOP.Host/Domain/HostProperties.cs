using Serilog.Events;
using System;

namespace MOP.Host.Domain
{
    internal class HostProperties
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = "DefaultHost";
        public LogEventLevel LogEventLevel { get; set; } = LogEventLevel.Debug;
        public string? TempDirectory { get; set; }
        public string? DataDirectory { get; set; }
        public bool WriteToConsole { get; set; } = true;
        public bool WriteToFile { get; set; } = true;
    }
}
