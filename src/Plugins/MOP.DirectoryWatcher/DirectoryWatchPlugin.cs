using System;
using Akka.Actor;
using MOP.Core.Domain.Events;
using MOP.Core.Domain.Plugins;
using MOP.Core.Services;
using MOP.DirectoryWatcher.Actors;
using MOP.DirectoryWatcher.Models;
using System.Threading.Tasks;

using static MOP.Core.Domain.Events.EventTypes;

namespace MOP.DirectoryWatcher
{
    public class DirectoryWatchPlugin : BaseActorPlugin<DirectoryWatchPlugin>
    {
        public DirectoryWatchPlugin(IInjectorService injector) : base(injector) { }

        public override string ActorRefName => "DirectoryWatch";

        protected override IPluginInfo BuildPluginInfo() => new Info();

        protected override async Task<Props> GetActorPropsAsync()
        {
            var configService = Injector.GetService<IConfigService>();
            var configResult = await configService.LoadConfig<DirectoryWatchConfig>(Info.Id);
            var config = configResult.ValueOr(new DirectoryWatchConfig());
            return Props.Create<SupervisorActor>(Injector, config);
        }
        protected async override Task<bool> OnInitAsync()
        {
            var res = await base.OnInitAsync();

            var actorSystem = Injector.GetService<ActorSystem>();
            EventService.Events.OfType(AddWatchDirectory, RemoveWatchDirectory)
                .Subscribe(e => actorSystem.ActorSelection(ActorPath).Tell(e));

            return res;
        }
    }
}
