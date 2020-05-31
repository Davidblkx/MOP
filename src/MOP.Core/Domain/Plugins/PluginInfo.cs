using Semver;
using System;

namespace MOP.Core.Domain.Plugins
{
    internal class PluginInfo : IPluginInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Namespace { get; }
        public SemVersion CoreVersion { get; set; }
        public ulong Priority { get; set; } = 1000;

        public PluginInfo()
        {
            Namespace = BuildNamespace();
        }

        private string BuildNamespace()
        {
            var id = Id.ToString().Replace("-", "");
            return $"[{id}]";
        }
    }
}
