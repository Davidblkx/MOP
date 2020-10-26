using MOP.Core.Infra;
using System;

namespace MOP.Core.Domain.Plugins
{
    public class PluginInfo : IPluginInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Namespace { get; set; }
        public MopVersion CoreVersion { get; set; }
        public ulong Priority { get; set; }
        public MopVersion Version { get; set; }

        public PluginInfo()
        {
            Id = Guid.NewGuid();
            Name = "Unknown";
            Namespace = BuildNamespace();
            CoreVersion = new MopVersion(1, 0, 0);
            Priority = PluginPriority.DEFAULT;
            Version = new MopVersion(0, 1, 0);
        }

        public PluginInfo(Guid id)
        {
            Id = id;
            Name = "Unknown";
            Namespace = BuildNamespace();
            CoreVersion = new MopVersion(1, 0, 0);
            Priority = PluginPriority.DEFAULT;
            Version = new MopVersion(0, 1, 0);
        }

        private string BuildNamespace()
        {
            var id = Id.ToString().Replace("-", "");
            return $"[{id}]";
        }

        public static PluginInfo From(IPluginInfo i)
        {
            if (i is PluginInfo info)
                return info;

            return new PluginInfo
            { 
                CoreVersion = i.CoreVersion,
                Id = i.Id,
                Name = i.Name,
                Namespace = i.Namespace,
                Priority = i.Priority,
                Version = i.Version,
            };
        }
    }
}
