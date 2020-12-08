using MOP.Core.Domain.Plugins;
using System;

namespace MOP.RemoteInterfaceProtocol
{
    internal class Info : PluginInfo
    {
        public const string NAME = "RemoteInterfaceProtocol";

        public Info() : base()
        {
            Id = Guid.Parse("81db850b-511d-4281-b05a-80c956ffa8af");
            Name = NAME;
            Namespace = BuildNamespace();
        }
    }
}
