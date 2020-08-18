using MOP.Terminal.CommandLine;
using MOP.Terminal.Settings;
using Newtonsoft.Json;
using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace MOP.Terminal.CommandLineTransformers
{
    public class InfoCommand : ICommandBuilderFactory
    {
        public Task<CommandLineBuilder> Transform(CommandLineBuilder builder)
        {
            var cmd = new Command("print")
            {
                Description = "print config for current instance using passed settings",
                Handler = CommandHandler.Create(PrintHandler)
            };

            return Task.Run(() => builder.AddCommand(cmd));
        }

        private void PrintHandler()
        {
            var settings = JsonConvert.SerializeObject(LocalSettings.Current, Formatting.Indented);
            Console.WriteLine(settings);
        }
    }
}
