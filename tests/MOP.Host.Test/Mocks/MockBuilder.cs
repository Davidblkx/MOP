using MOP.Core.Domain.Host;
using MOP.Core.Services;
using MOP.Host.Domain;
using MOP.Host.Services;
using System;
using System.Threading;

namespace MOP.Host.Test.Mocks
{
    public static class MockBuilder
    {
        public static ILogService BuildLogService()
        {
            return new LogService(BuildMockHostProps());
        }

        public static IHost BuildHost()
        {
            var host = new MopHost(BuildMockHostProps(), new CancellationToken());
            host.SetLogService(BuildLogService());
            return host;
        }

        public static void CleanDirectory()
        {
            var host = BuildHost();
            if (host.DataDirectory.Exists)
                host.DataDirectory.Delete(true);
            if (host.TempDirectory.Exists)
                host.TempDirectory.Delete(true);
        }

        private static HostProperties BuildMockHostProps()
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
    }
}
