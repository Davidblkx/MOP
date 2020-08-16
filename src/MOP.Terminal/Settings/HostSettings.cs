namespace MOP.Terminal.Settings
{
    internal class HostSettings : IHostSettings
    {
        public int Port { get; set; }

        public string Hostname { get; set; } = "localhost";

        public string Name { get; set; } = "Unknown";
    }
}
