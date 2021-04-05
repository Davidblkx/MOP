using System;
using MOP.Core.Domain.Plugins;

namespace MOP.Ping
{
    internal class Info : PluginInfo
    {
        public const string NAME = "Ping";

        public Info() : base()
        {
            Id = Guid.Parse("07a80c3a-cd29-42e0-b05f-ebbdd4d8efb8");
            Name = NAME;
            Namespace = BuildNamespace();
        }
    }
}
