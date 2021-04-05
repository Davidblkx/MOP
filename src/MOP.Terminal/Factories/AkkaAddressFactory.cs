using System.Collections.Generic;
using System.Linq;
using System.Text;
using MOP.Core.Infra.Extensions;
using MOP.Terminal.Models;

namespace MOP.Terminal.Factories
{
    public class AkkaAddressFactory
    {
        private readonly StringBuilder _base;
        private readonly List<string> _paths;

        public AkkaAddressFactory(IHostConfig host, string protocol = "tcp")
        {
            _base = new StringBuilder(BuildBaseURL(host, protocol));
            _paths = new List<string>() { "user" };
        }

        public AkkaAddressFactory AddPath(params string[] paths)
        {
            var pathList = paths
                .Select(e => FormatPath(e))
                .Where(e => !e.IsNullOrEmpty());
            _paths.AddRange(pathList);
            return this;
        }

        public string Build()
        {
            return _base
                .Append('/')
                .Append(_paths.Join("/"))
                .ToString();
        }

        private static string FormatPath(string input)
        {
            if (input.IsNullOrEmpty()) return input;
            return input
                .TrimStart('/')
                .TrimEnd('/');
        }

        private static string BuildBaseURL(IHostConfig host, string protocol)
            => $"akka.{protocol}://{host.Id}@{host.Hostname}:{host.Port}";

        public static string BuildTcp(IHostConfig host, params string[] paths)
            => new AkkaAddressFactory(host, "tcp")
                .AddPath(paths)
                .Build();
    }
}
