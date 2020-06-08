using MOP.Core.Services;
using MOP.Host.Domain;
using MOP.Host.Helpers;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MOP.Host.Services
{
    internal class LogService : ILogService
    {
        private ILogger? _logger;

        List<Func<LoggerConfiguration, LoggerConfiguration>> _loggerTransformers
            = new List<Func<LoggerConfiguration, LoggerConfiguration>>();
        
        public LogService(HostProperties p)
        {
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
                _logger = BuildLogger();
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
        }

        private void ConfigureFileLogger(HostProperties p)
        {
            var dir = new DirectoryInfo(p.DataDirectory);
            var logFile = dir.RelativeFile("log.txt");

            _loggerTransformers.Add(e =>
                e.WriteTo.File(logFile.FullName));
        }
    }
}
