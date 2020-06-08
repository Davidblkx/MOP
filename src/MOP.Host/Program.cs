using System;
using System.Threading;
using System.Threading.Tasks;

namespace MOP.Host
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var cancelToken = new CancellationTokenSource();
            var host = await MopHost.BuildHost(args, cancelToken.Token);
            host.BeforeExit += (sender, code) => Console.WriteLine($"Exit called with code {code}");
            host.Exit += (_, e) => cancelToken.Cancel();
            return await host.Start();
        }
    }
}
