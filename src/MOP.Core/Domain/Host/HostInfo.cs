using Semver;
using System;

namespace MOP.Core.Domain.Host
{
    internal class HostInfo : IHostInfo
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public SemVersion CoreVersion { get; set; }
    }
}
