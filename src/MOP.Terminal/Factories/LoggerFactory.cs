using Serilog;
using MOP.Terminal.Models;

namespace MOP.Terminal.Factories
{
    /// <summary>
    /// Create instance of ILogger for current settings
    /// </summary>
    public static class LoggerFactory
    {
        public static ILogger Build(ITerminalSettings settings)
        {
            var config = new LoggerConfiguration();

            if (settings.LogLevel <= 0)
                return config.CreateLogger();

            if (settings.LogToFile)
                config = config.WriteTo.File("MOPTerminal.log");

            config = ConfigLogLevel(config, settings.LogLevel);

            return config
                .CreateLogger();
        }

        private static LoggerConfiguration ConfigLogLevel(LoggerConfiguration config, int level)
        {
            return level switch
            {
                1 => config.MinimumLevel.Fatal(),
                2 => config.MinimumLevel.Error(),
                3 => config.MinimumLevel.Warning(),
                4 => config.MinimumLevel.Information(),
                5 => config.MinimumLevel.Verbose(),
                _ => config.MinimumLevel.Error(),
            };
        }
    }
}
