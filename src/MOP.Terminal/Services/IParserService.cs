using System.Threading.Tasks;

namespace MOP.Terminal.Services
{
    internal interface IParserService
    {
        Task<int> InvokeAsync(string[] args);
        Task<int> InvokeAsync(string args);

        int Invoke(string[] args);
        int Invoke(string args);
    }
}
