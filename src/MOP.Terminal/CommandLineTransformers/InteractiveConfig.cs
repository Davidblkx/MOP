using MOP.Terminal.CommandLine;
using MOP.Terminal.ConsoleEmulator;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace MOP.Terminal.CommandLineTransformers
{
    public class InteractiveConfig : ICommandBuilderFactory
    {
        public Task<CommandLineBuilder> Transform(CommandLineBuilder builder)
        {
            return Task.Run(() => builder
                .AddGlobalOption(BuildInteractiveOption())
            );
        }

        private Option<bool> BuildInteractiveOption()
        {
            var cmd = new Option<bool>(new string[] { "--interactive", "-i" })
            {
                Description = "Start terminal in interactive mode",
            };
            return cmd;
        }
    }
}
