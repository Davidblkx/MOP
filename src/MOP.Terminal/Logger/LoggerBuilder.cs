using MOP.Terminal.Settings;
using Serilog;

namespace MOP.Terminal.Logger
{
    internal class LoggerBuilder
    {
        public static ILogger Build(ISettings settings)
        {
            var config = new LoggerConfiguration();

            if (settings.LogLevel <= 0)
                return config.CreateLogger();

            if (settings.LogToFile)
                config = config.WriteTo.File("MOPTerminal.log");

            config = ConfigLogLevel(config, settings.LogLevel);

            return config
                .WriteTo.Console()
                .CreateLogger();
        }

        private static LoggerConfiguration ConfigLogLevel(LoggerConfiguration config, int level)
        {
            switch (level)
            {
                case 1:
                    return config.MinimumLevel.Fatal();
                case 2:
                    return config.MinimumLevel.Error();
                case 3:
                    return config.MinimumLevel.Warning();
                case 4:
                    return config.MinimumLevel.Information();
                case 5:
                    return config.MinimumLevel.Verbose();

                default:
                    return config.MinimumLevel.Error();
            }
        }
    }
}
