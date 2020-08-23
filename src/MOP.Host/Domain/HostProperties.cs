using MOP.Core.Akka.Hocon;
using Serilog.Events;
using System;

namespace MOP.Host.Domain
{
    public class HostProperties
    {
        public Guid Id { get; set; } = Guid.Parse("f777f44d-3e0a-41a6-8f2c-1efe9d0a78d7");
        public string Name { get; set; } = "DefaultHost";
        public LogEventLevel LogEventLevel { get; set; } = LogEventLevel.Debug;
        public string? TempDirectory { get; set; }
        public string? DataDirectory { get; set; }
        public bool WriteToConsole { get; set; } = true;
        public bool WriteToFile { get; set; } = true;
        public bool AllowRemote { get; set; } = true;
        public HoconConfig ActorSystemConfig { get; set; }

        public HostProperties()
        {
            ActorSystemConfig = new HoconConfig();
        }
    }
}
