using MOP.Terminal.CommandLine;
using System.CommandLine;
using System.CommandLine.Builder;
using System.Threading.Tasks;

namespace MOP.Terminal.CommandLineTransformers
{
    internal class SettingsConfig : ICommandBuilderFactory
    {
        public Task<CommandLineBuilder> Transform(CommandLineBuilder builder)
        {
            return Task.Run(() => builder
                .AddGlobalOption(BuildSettingsPathOption())
                .AddGlobalOption(BuildSettingsHostOption())
                .AddGlobalOption(BuildVerboseLevelOption())
                .AddGlobalOption(BuildSettingsReloadOption())
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

        private Option BuildSettingsReloadOption()
        {
            return new Option<bool>(
                new string[] { "--reload-settings" },
                description: "Force settings to be reloaded"
            );
        }

        private Option BuildSettingsHostOption()
        {
            return new Option<string>(
                new string[] { "--host", "-H" },
                getDefaultValue: () => "",
                description: "Name of host to use"
            );
        }

        private Option BuildVerboseLevelOption()
        {
            return new Option<int>(
                new string[] { "--verbose", "-V" },
                getDefaultValue: () => -1,
                description: "Verbose level to show, max 5, 0 to disable output, -1 to use settings config"
            );
        }
    }
}
