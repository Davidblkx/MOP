using System.Threading.Tasks;

namespace MOP.Terminal.Services
{
    public interface ISettingsLoaderService<T> where T : new()
    {
        Task<T> Load();
        Task<bool> Save(T value);
    }
}