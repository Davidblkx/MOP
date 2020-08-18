using MOP.Infra.Domain.Host;
using MOP.Infra.Domain.Plugins;
using System;

namespace MOP.Infra.Tools
{
    /// <summary>
    /// Helper to build HOST info
    /// </summary>
    public static class InfoBuilder
    {
        public static IHostInfo BuildHostInfo(Guid id, string name)
        {
            return new HostInfo
            {
                Id = id,
                Name = name,
                CoreVersion = MopHelper.GetCoreVersion()
            };
        }

        public static IPluginInfo BuildPluginInfo(Guid id, string name)
        {
            return new PluginInfo
            {
                Id = id,
                Name = name,
                CoreVersion = MopHelper.GetCoreVersion()
            };
        }
    }
}
