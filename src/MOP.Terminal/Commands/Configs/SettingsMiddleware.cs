using System;
using MOP.Core.Helpers;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using System.IO;
using MOP.Terminal.Settings;
using MOP.Terminal.ConsoleEmulator;

namespace MOP.Terminal.Commands.Configs
{
    public class SettingsMiddleware : ICommandConfig
    {
        public Task<CommandLineBuilder> ApplyConfig(CommandLineBuilder builder)
        {
            return Task.Run(() => builder
                .UseMiddleware(LoadSettingsMiddleware)
                .UseMiddleware(DefaultCommandMiddleware)
            );
        }

        public async Task LoadSettingsMiddleware(
            InvocationContext context, Func<InvocationContext, Task> next)
        {
            var val = context.ParseResult.ValueForOption<string>("--settings");
            if (!val.IsNullOrEmpty())
            {
                var file = new FileInfo(val);
                SettingsHandler.ReloadInstance(file);
            }

            await LocalSettings.ReloadSettings();
            await next(context);
        }

        public async Task DefaultCommandMiddleware(
            InvocationContext context, Func<InvocationContext, Task> next)
        {
            var cmd = context.ParseResult.CommandResult;
            if (cmd.Tokens.Count == 0)
            {
                await Emulator.Execute();
                return;
            }

            await next(context);
        }
    }
}
