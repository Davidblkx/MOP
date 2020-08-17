using System.CommandLine;

namespace MOP.Terminal.Commands
{
    public static class CommandBuilder
    {
        public static RootCommand Build()
        {
            var rootCommand = new RootCommand
            {

            };

            rootCommand.Description = "Terminal for MOP";

            return rootCommand;
        }
    }
}
