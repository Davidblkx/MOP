using Serilog;

namespace MOP.Terminal.Services
{
    public interface ILogService
    {
        ILogger ForContext<T>();
    }
}
