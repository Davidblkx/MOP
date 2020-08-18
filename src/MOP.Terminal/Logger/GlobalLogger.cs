using MOP.Terminal.Settings;
using Serilog;
using System;

namespace MOP.Terminal.Logger
{
    internal static class GlobalLogger
    {
        private static ILogger? _mainLogger;
        public static ILogger Log
        {
            get
            {
                if (_mainLogger is null)
                    throw new ArgumentNullException("Logger not initialized");
                return _mainLogger;
            }
        }

        public static void InitLogger(ISettings? settings = null)
        {
            ISettings s = settings ?? LocalSettings.Current;
            _mainLogger = LoggerBuilder.Build(s);
        }

        public static ILogger ForContext<T>()
            => Log.ForContext<T>();
    }
}
