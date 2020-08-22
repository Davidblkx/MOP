using MOP.Core.Infra;
using System;

namespace MOP.Infra.Domain.Plugins
{
    public class PluginInfo : IPluginInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Namespace { get; set; }
        public MopVersion CoreVersion { get; set; }
        public ulong Priority { get; set; }

        public PluginInfo()
        {
            Id = Guid.NewGuid();
            Name = "Unknown";
            Namespace = BuildNamespace();
            CoreVersion = new MopVersion(1, 0, 0);
            Priority = 1000;
        }

        private string BuildNamespace()
        {
            var id = Id.ToString().Replace("-", "");
            return $"[{id}]";
        }
    }
}
