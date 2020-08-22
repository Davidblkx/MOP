using MOP.Core.Infra;
using System;

namespace MOP.Infra.Domain.Plugins
{
    internal class PluginInfo : IPluginInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "Unknown";
        public string Namespace => BuildNamespace();
        public MopVersion CoreVersion { get; set; } = new MopVersion();
        public ulong Priority { get; set; } = 1000;

        private string BuildNamespace()
        {
            var id = Id.ToString().Replace("-", "");
            return $"[{id}]";
        }
    }
}
