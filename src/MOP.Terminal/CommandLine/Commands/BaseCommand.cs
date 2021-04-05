using MOP.Terminal.Infra;
using MOP.Terminal.Services;
using System.CommandLine.Facilitator;

namespace MOP.Terminal.CommandLine.Commands
{
    abstract class BaseCommand<T> where T : new()
    {
        [Handler]
        public void HandleCommand(T value)
        {
            // DependencyInjector.GetInstance<IActorService>().CallCommand(value);
        }
    }
}
