using System;
using System.Collections.Generic;
using System.Text;

namespace MOP.Terminal.Settings
{
    public interface IHostSettings
    {
        int Port { get; }
        string Hostname { get; }
        string Name { get; }
        Guid Id { get; }
    }
}
