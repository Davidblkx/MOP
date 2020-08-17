using System.CommandLine;
using System.CommandLine.Builder;
using System.Threading.Tasks;

namespace MOP.Terminal.Commands.Configs
{
    internal class SettingsConfig : ICommandConfig
    {
        public Task<CommandLineBuilder> ApplyConfig(CommandLineBuilder builder)
        {
            return Task.Run(() => builder
                .AddGlobalOption(BuildSettingsPathOption())
                .AddGlobalOption(BuildSettingsHostOption())
                .AddGlobalOption(BuildVerboseLevelOption())
            );
        }

        private Option BuildSettingsPathOption()
        {
            return new Option<string>(
                new string[] { "--settings", "-s" },
                getDefaultValue: () => "",
                description: "Path to .json file with settings to load"
            );
        }

        private Option BuildSettingsHostOption()
        {
            return new Option<string>(
                new string[] { "--host", "-h" },
                getDefaultValue: () => "",
                description: "Name of host to use"
            );
        }

        private Option BuildVerboseLevelOption()
        {
            return new Option<int>(
                new string[] { "--verbose", "-V" },
                getDefaultValue: () => 5,
                description: "Verbose level to show, max 5, 0 to disable output"
            );
        }
    }
}
