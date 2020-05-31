using MOP.Core.Domain.Host;
using MOP.Core.Domain.Plugins;
using System;

namespace MOP.Core.Helpers
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
