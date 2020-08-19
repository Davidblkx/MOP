using System;

namespace MOP.Terminal.Settings
{
    public class HostSettings : IHostSettings
    {
        public int Port { get; set; }

        public string Hostname { get; set; } = "localhost";

        public string Name { get; set; } = "Unknown";

        public Guid Id { get; set; } = Guid.NewGuid();

        public static HostSettings From(IHostSettings s)
        {
            if (s is HostSettings settings)
                return settings;

            return new HostSettings
            {
                Hostname = s.Hostname,
                Id = s.Id,
                Name = s.Name,
                Port = s.Port
            };
        }

        public static HostSettings From(string name, string hostname, int port, Guid id)
            => new HostSettings
            {
                Hostname = hostname,
                Id = id,
                Name = name,
                Port = port
            };
    }
}
