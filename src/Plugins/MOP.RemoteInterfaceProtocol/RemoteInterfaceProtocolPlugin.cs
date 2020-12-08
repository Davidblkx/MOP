using Akka.Actor;
using MOP.Core.Domain.Plugins;
using MOP.Core.Services;
using System.Threading.Tasks;

namespace MOP.RemoteInterfaceProtocol
{
    public class RemoteInterfaceProtocolPlugin : BaseActorPlugin<RemoteInterfaceProtocolPlugin>
    {
        private readonly IRIPService _rip;

        public override string ActorRefName => "RIP";

        public RemoteInterfaceProtocolPlugin(IInjectorService injector): base(injector)
        {
            _rip = Injector.GetService<IRIPService>();
            Injector.RegisterService<HelperService>(LifeCycle.Singleton);
            _rip.Register<HelperService>();
        }

        protected override IPluginInfo BuildPluginInfo()
            => new Info();

        protected override Task<Props> GetActorPropsAsync()
        {
            var logger = Injector.GetService<ILogService>();
            var props = Props.Create(() => new RemoteInterfaceProtocolActor(logger, _rip, Injector));
            return Task.FromResult(props);
        }
    }
}
