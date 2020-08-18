using System;
using MOP.Infra.Extensions;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using System.IO;
using MOP.Terminal.Settings;
using MOP.Terminal.ConsoleEmulator;
using MOP.Terminal.Logger;

namespace MOP.Terminal.Commands.Configs
{
    public class MiddlewareConfig : ICommandConfig
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
            GlobalLogger.InitLogger();
            await next(context);
        }

        /// <summary>
        /// Starts in interactive mode if no command is provided.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="next">The next.</param>
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
