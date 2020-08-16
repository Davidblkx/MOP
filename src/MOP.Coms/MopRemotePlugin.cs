using MOP.Core.Domain.Host;
using MOP.Core.Domain.Plugins;
using MOP.Core.Services;
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
                var service = Host?.LogService;
                var info = Host?.Info;
                if (service is null || info is null) throw new ArgumentNullException("Log service is null");
                return InitActor(service, info);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error starting MOP.Remote actor");
                return false;
            }
        }

        private bool InitActor(ILogService service, IHostInfo info)
        {
            var factory = new RemoteActorFactory(service, info);
            Host?.ActorService?.AddActorFactory(factory);
            return Host?.ActorService?
                .GetActorOf(factory.ActorRefName)
                .Map(_ => true)
                .ValueOr(false) ?? false;
        }
                
    }
}
