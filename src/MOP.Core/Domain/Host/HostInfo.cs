using Semver;
using System;

namespace MOP.Infra.Domain.Host
{
    public class HostInfo : IHostInfo
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = "";

        public SemVersion CoreVersion { get; set; } = new SemVersion(0,1,0);
    }
}
