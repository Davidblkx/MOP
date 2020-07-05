using MOP.Core.Domain.Host;
using MOP.Core.Services;
using MOP.Host.Domain;
using MOP.Host.Events;
using MOP.Host.Helpers;
using MOP.Host.Services;
using Serilog;
using System;
using System.IO;
using System.Threading;

namespace MOP.Host.Test.Mocks
{
    public class MockBuilder : IDisposable
    {
        public IHost Host { get; }
        public IEventStorage Storage { get; }

        private FileInfo db;

        public MockBuilder()
        {
            Host = BuildHost();
            Storage = BuildStorage();
        }

        private ILogService BuildLogService()
        {
            return new LogService(BuildMockHostProps());
        }

        private IHost BuildHost()
        {
            var host = new MopHost(BuildMockHostProps(), new CancellationToken());
            host.SetLogService(BuildLogService());
            host.SetConfigService(new ConfigService(host));
            return host;
        }

        private IEventStorage BuildStorage()
        {
            db = Host.DataDirectory.RelativeFile("events.sb");
            return new EventStorage(db, Host.LogService);
        }

        private void CleanDirectory()
        {
            try
            {
                if (Host.LogService is LogService e)
                    e.Dispose();
                Log.CloseAndFlush();
                if (db?.Exists ?? false)
                    db?.Delete();
                if (Host.DataDirectory.Exists)
                    Host.DataDirectory.Delete(true);
                if (Host.TempDirectory.Exists)
                    Host.TempDirectory.Delete(true);
            } catch (Exception e) { }
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
            };
        }

        public void Dispose()
        {
            CleanDirectory();
        }
    }
}
