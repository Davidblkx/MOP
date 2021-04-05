using MOP.Terminal.Models;
using System.Threading.Tasks;

namespace MOP.Terminal.Services
{
    internal interface ISettingsService : ITerminalSettings
    {
        Task<bool> LoadAsync();
        Task<bool> SaveAsync();
    }
}