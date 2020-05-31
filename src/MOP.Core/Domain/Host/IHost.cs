using MOP.Core.Services;

namespace MOP.Core.Domain.Host
{
    public interface IHost
    {
        IHostInfo Info { get; }

        IActorService ActorService { get; }
        IEventService EventService { get; }
        ILogService LogService { get; }
    }
}
