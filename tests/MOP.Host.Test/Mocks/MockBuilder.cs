using MOP.Core.Domain.Host;
using MOP.Core.Services;
using MOP.Host.Domain;
using MOP.Host.Services;
using Serilog;
using System;
using System.IO;
using System.Threading;
using MOP.Core.Infra.Extensions;
using MOP.Core.Infra;
using MOP.Core.Domain.Plugins;

namespace MOP.Host.Test.Mocks
{
    public class MockBuilder : IDisposable
    {
        public static readonly IInjectorService injector = new InjectorService(false);

        public IHost Host { get; }

        private FileInfo db;

        public MockBuilder()
        {
            Host = BuildHost();
        }

        private IHost BuildHost()
        {
            injector.RegisterService(() => BuildMockHostProps(), LifeCycle.Singleton);
            injector.RegisterService(() => new MopLifeService(new CancellationToken()), LifeCycle.Singleton);
            injector.RegisterService(() => injector, LifeCycle.Singleton);
            injector.RegisterService<IHost, MopHost>(LifeCycle.Singleton);
            injector.RegisterService<ILogService, LogService>(LifeCycle.Singleton);
            injector.RegisterService<IConfigService, ConfigService>(LifeCycle.Singleton);

            if (injector.GetService<IHost>() is MopHost host)
                return host;

            throw new Exception("Failed to instantiate host");
        }

        private void CleanDirectory()
        {
            try
            {
                if (injector.GetService<ILogService>() is LogService e)
                    e.Dispose();
                Log.CloseAndFlush();
                if (db?.Exists ?? false)
                    db?.Delete();
                if (Host.DataDirectory.Exists)
                    Host.DataDirectory.Delete(true);
                if (Host.TempDirectory.Exists)
                    Host.TempDirectory.Delete(true);
            } catch { }
        }

        private HostProperties BuildMockHostProps()
        {
            return new HostProperties
            {
                DataDirectory = "test-data",
                TempDirectory = "test-temp",
                Id = Guid.Parse("7efb109b-0e58-46fb-b88b-693dbff2041a"),
                LogEventLevel = Serilog.Events.LogEventLevel.Verbose,
                Name = "TestHost",
                WriteToConsole = true,
                WriteToFile = true,
                AllowRemote = true,
            };
        }

        public void Dispose()
        {
            CleanDirectory();
        }
    }
}
