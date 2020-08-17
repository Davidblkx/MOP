using MOP.Terminal.Commands;
using MOP.Terminal.ConsoleEmulator;
using System.CommandLine;
using System.Threading.Tasks;

namespace MOP.Terminal
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var root = CommandBuilder.Build();
            if (args.Length > 0)
                return await root.InvokeAsync(args);

            await Emulator.Execute(root);
            return 0;
        }
    }
}
