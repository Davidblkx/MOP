using Akka.Actor;
using MOP.Core.Services;
using System.Threading.Tasks;

namespace MOP.Core.Domain.Plugins
{
    public abstract class BaseActorPlugin<T> : BasePlugin<T>
    {
        public abstract string ActorRefName { get; }

        public BaseActorPlugin(IInjectorService injector) : base(injector) { }

        protected async override Task<bool> OnInitAsync()
        {
            Logger.Debug("Creating actor {@ActorRefName} for plugin {@FullName}", ActorRefName, FullName);
            var system = Injector.GetService<ActorSystem>();
            system.ActorOf(await GetActorPropsAsync(), ActorRefName);
            Logger.Debug("Actor {@ActorRefName} created for plugin {@FullName}", ActorRefName, FullName);
            return true;
        }

        protected void RegisterRoles(params string[] roles) 
        {
            // TODO: implement logic to register new roles
        }

        protected abstract Task<Props> GetActorPropsAsync();
    }
}
