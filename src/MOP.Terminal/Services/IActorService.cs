using Akka.Actor;
using System.Threading.Tasks;

namespace MOP.Terminal.Services
{
    internal interface IActorService
    {
        ActorSystem ActorSystem { get; }

        void Start(string[] args);

        Task Shutdown();

        void Ping(object? message, IActorRef? Sender = null);
    }
}