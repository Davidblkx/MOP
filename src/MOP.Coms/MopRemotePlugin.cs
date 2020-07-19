using MOP.Core.Domain.Plugins;

namespace MOP.Remote
{
    public class MopRemotePlugin : Plugin<MopRemotePlugin>
    {
        private const string GUID = "4a229ba5-0ded-447c-927c-a849bc78c082";

        public MopRemotePlugin() : base(GUID, "Mop Remote", PluginPriority.CORE)
        { }
    }
}
