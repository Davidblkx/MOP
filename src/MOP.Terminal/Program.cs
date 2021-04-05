using MOP.Core.Infra;
using MOP.Terminal.Models;
using MOP.Terminal.Services;
using System.Threading.Tasks;

using static MOP.Terminal.Infra.DependencyInjector;

namespace MOP.Terminal
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var newArgs = Register(args);

            var actorService = GetInstance<IActorService>();
            actorService.Start(newArgs);

            await GetInstance<MopLifeService>().WaitForExit();
            await actorService.Shutdown();

            return AppState.Result;
        }
    }
}
