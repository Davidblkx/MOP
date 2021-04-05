using System.CommandLine;
using System.Threading.Tasks;

namespace MOP.Terminal.Services
{
    internal interface ICommandLineService
    {
        RootCommand RootCommand { get; }

        Task<int> Invoke(string[] args);
    }
}