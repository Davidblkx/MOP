using MOP.Core.Infra;
using System;

namespace MOP.Core.Domain.Host
{
    public class HostInfo : IHostInfo
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = "";

        public MopVersion CoreVersion { get; set; } = new MopVersion(0,1,0);
    }
}
