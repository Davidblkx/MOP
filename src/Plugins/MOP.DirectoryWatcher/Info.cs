using System;
using MOP.Core.Domain.Plugins;

namespace MOP.DirectoryWatcher
{
    public class Info : PluginInfo
    {
        public const string ID = "e0dfc3ce-c683-43b8-9576-ea83c495e28f";

        public Info()
        {
            Id = Guid.Parse(ID);
            Name = "DirectoryWatch";
            Namespace = BuildNamespace();
        }
    }
}
