using MOP.Terminal.ConsoleEmulator;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace MOP.Terminal.Commands.Configs
{
    public class InteractiveConfig : ICommandConfig
    {
        public Task<CommandLineBuilder> ApplyConfig(CommandLineBuilder builder)
        {
            return Task.Run(() => builder
                .AddCommand(BuildEmulatorCommand())
            );
        }

        private Command BuildEmulatorCommand()
        {
            var cmd = new Command("interactive")
            {
                Description = "Start terminal in interactive mode",
                Handler = CommandHandler.Create(() => Emulator.Execute()),
            };
            cmd.AddAlias("i");
            return cmd;
        }
    }
}
