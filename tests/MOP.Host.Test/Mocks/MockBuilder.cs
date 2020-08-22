using MOP.Infra.Domain.Host;
using MOP.Infra.Services;
using MOP.Host.Domain;
using MOP.Host.Events;
using MOP.Host.Services;
using Serilog;
using System;
using System.IO;
using System.Threading;
using MOP.Core.Infra.Extensions;
using MOP.Core.Infra;
using MOP.Core.Services;
using MOP.Core.Domain.Plugins;

namespace MOP.Host.Test.Mocks
{
    public class MockBuilder : IDisposable
    {
        public static readonly IInjectorService injector = new InjectorService();

        public IHost Host { get; }
        public IEventStorage Storage { get; }

        private FileInfo db;

        public MockBuilder()
        {
            Host = BuildHost();
            Storage = BuildStorage();
        }

        private IHost BuildHost()
        {
            injector.RegisterService(() => BuildMockHostProps(), LifeCycle.Singleton);
            injector.RegisterService(() => new MopLifeService(new CancellationToken()), LifeCycle.Singleton);
            injector.RegisterService(() => injector, LifeCycle.Singleton);
            injector.RegisterService<IHost, MopHost>(LifeCycle.Singleton);
            injector.RegisterService<ILogService, LogService>(LifeCycle.Singleton);
            injector.RegisterService<IConfigService, ConfigService>(LifeCycle.Singleton);
            injector.RegisterService<IActorService, ActorService>(LifeCycle.Singleton);

            if (injector.GetService<IHost>() is MopHost host)
                return host;

            throw new Exception("Failed to instantiate host");
        }

        private IEventStorage BuildStorage()
        {
            db = Host.DataDirectory.RelativeFile("events.sb");
            return new EventStorage(db, injector.GetService<ILogService>());
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
                RemoteHostname = "localhost",
                RemotePort = 0
            };
        }

        public void Dispose()
        {
            CleanDirectory();
        }
    }
}
