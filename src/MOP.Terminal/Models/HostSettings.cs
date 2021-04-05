using System;

namespace MOP.Terminal.Models
{
    public class HostConfig : IHostConfig
    {
        public int Port { get; set; }

        public string Hostname { get; set; } = "localhost";

        public string Name { get; set; } = "Unknown";

        public Guid Id { get; set; } = Guid.NewGuid();

        public static HostConfig From(IHostConfig s)
        {
            if (s is HostConfig settings)
                return settings;

            return new HostConfig
            {
                Hostname = s.Hostname,
                Id = s.Id,
                Name = s.Name,
                Port = s.Port
            };
        }

        public static HostConfig From(string name, string hostname, int port, Guid id)
            => new HostConfig
            {
                Hostname = hostname,
                Id = id,
                Name = name,
                Port = port
            };
    }
}
