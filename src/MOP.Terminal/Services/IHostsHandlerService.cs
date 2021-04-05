using MOP.Terminal.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MOP.Terminal.Services
{
    public interface IHostsHandlerService
    {
        HostConfig? FindHost(Guid id);
        HostConfig? FindHost(string name);
        IEnumerable<HostConfig> GetHostConfigs();
        bool HasHost(string name);
        Task RemoveHost(string name);
        Task SetHost(IHostConfig config, bool isDefault);
    }
}