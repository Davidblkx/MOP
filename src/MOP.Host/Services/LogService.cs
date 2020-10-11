using MOP.Core.Services;
using MOP.Host.Domain;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using MOP.Core.Infra.Extensions;

[assembly: InternalsVisibleTo("MOP.Host.Test")]
namespace MOP.Host.Services
{
    internal class LogService : ILogService, IDisposable
    {
        private ILogger? _logger;

        private readonly List<Func<LoggerConfiguration, LoggerConfiguration>> _loggerTransformers;
        
        public LogService(HostProperties p)
        {
            _loggerTransformers = new List<Func<LoggerConfiguration, LoggerConfiguration>>();
            ConfigureLogger(p);
        }

        public void AddLoggerTransformer(Func<LoggerConfiguration, LoggerConfiguration> fn)
        {
            _loggerTransformers.Add(fn);
            _logger = BuildLogger();
        }

        public ILogger GetContextLogger<T>()
            => GetLogger().ForContext<T>();

        public ILogger GetLogger()
        {
            if (_logger is null)
            {
                _logger = BuildLogger();
                Log.Logger = _logger;
            }
            return _logger;
        }

        private ILogger BuildLogger()
        {
            return _loggerTransformers
                .Aggregate(new LoggerConfiguration(), (acc, fn) => fn(acc))
                .CreateLogger();
        }

        private void ConfigureLogger(HostProperties p)
        {
            if (p.WriteToConsole)
                _loggerTransformers.Add(e => e.WriteTo.Console());
            if (p.WriteToFile)
                ConfigureFileLogger(p);

            _loggerTransformers.Add(e => e.MinimumLevel.Is(p.LogEventLevel));
        }

        private void ConfigureFileLogger(HostProperties p)
        {
            var dir = new DirectoryInfo(p.DataDirectory);
            var logFile = dir.RelativeFile("log.txt");

            _loggerTransformers.Add(e =>
                e.WriteTo.File(logFile.FullName));
        }

        public void Dispose()
        {
            if (_logger is Logger e)
                e.Dispose();
        }
    }
}
