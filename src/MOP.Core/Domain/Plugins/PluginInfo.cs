using Semver;
using System;

namespace MOP.Infra.Domain.Plugins
{
    internal class PluginInfo : IPluginInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "Unknown";
        public string Namespace => BuildNamespace();
        public SemVersion CoreVersion { get; set; } = new SemVersion(0,1,0);
        public ulong Priority { get; set; } = 1000;

        private string BuildNamespace()
        {
            var id = Id.ToString().Replace("-", "");
            return $"[{id}]";
        }
    }
}
