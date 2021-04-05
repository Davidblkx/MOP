using System;

namespace MOP.Terminal.Models
{
    public interface IHostConfig
    {
        int Port { get; }
        string Hostname { get; }
        string Name { get; }
        Guid Id { get; }
    }
}
