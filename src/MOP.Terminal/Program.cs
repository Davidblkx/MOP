using MOP.Terminal.CommandLine;
using MOP.Terminal.ConsoleEmulator;
using System.CommandLine.Parsing;
using System.Threading.Tasks;

namespace MOP.Terminal
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var parser = await ParserBuilder.Build();
            Emulator.CommonParser = parser;
            return await parser.InvokeAsync(args);
        }
    }
}
