using Akka.Actor;
using MOP.Core.Domain.Plugins;
using MOP.Core.Services;
using System.Threading.Tasks;

namespace MOP.Ping
{
    public class PingPlugin : BaseActorPlugin<PingActor>
    {
        public override string ActorRefName => "ping";
        private readonly ILogService _log;

        public PingPlugin(IInjectorService injector) : base(injector)
        {
            _log = injector.GetService<ILogService>();
            RegisterRoles("ping");
        }

        protected override IPluginInfo BuildPluginInfo() => new Info();

        protected override Task<Props> GetActorPropsAsync()
            => Task.FromResult(Props.Create<PingActor>(_log));
    }
}
