using MOP.Terminal.Commands.Configs;
using System.Collections.Generic;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.Threading.Tasks;

namespace MOP.Terminal.Commands
{
    public static class CommandBuilder
    {
        public static async Task<Parser> Build()
        {
            var builder = new CommandLineBuilder();
            foreach(var config in GetCommandConfigs())
            {
                builder = await config.ApplyConfig(builder);
            }
            return builder.Build();
        }

        public static IEnumerable<ICommandConfig> GetCommandConfigs()
        {
            return new List<ICommandConfig>
            {
                new SettingsConfig(),
                new InteractiveConfig(),
                new MiddlewareConfig(),
            };
        }
    }
}
