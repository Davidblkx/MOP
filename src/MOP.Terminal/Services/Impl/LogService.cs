using MOP.Terminal.Factories;
using Serilog;

namespace MOP.Terminal.Services
{
    internal class LogService : ILogService
    {
        private readonly ILogger _mainLogger;

        public LogService(ISettingsService settings)
        {
            _mainLogger = LoggerFactory.Build(settings);
        }

        public ILogger ForContext<T>()
            => _mainLogger.ForContext<T>();
    }
}
