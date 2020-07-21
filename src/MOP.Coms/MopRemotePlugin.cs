using MOP.Core.Domain.Plugins;
using Optional.Unsafe;
using Serilog;
using System;
using System.Threading.Tasks;

namespace MOP.Remote
{
    public class MopRemotePlugin : Plugin<MopRemotePlugin>
    {
        public const string GUID = "4a229ba5-0ded-447c-927c-a849bc78c082";
        public static Guid GUID_VALUE => Guid.Parse(GUID);

        public MopRemotePlugin() : base(GUID, "Mop Remote", PluginPriority.CORE)
        { }

        public override async Task<bool> Initialize()
        {
            if (! await base.Initialize()) return false;

            try
            {
                await InitActor();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error starting MOP.Remote actor");
                return false;
            }

            return true;
        }

        private async Task InitActor()
        {

        }
                
    }
}
