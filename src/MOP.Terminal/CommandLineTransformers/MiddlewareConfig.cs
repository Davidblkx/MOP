using System;
using MOP.Infra.Extensions;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using System.IO;
using MOP.Terminal.Settings;
using MOP.Terminal.ConsoleEmulator;
using MOP.Terminal.Logger;
using MOP.Terminal.CommandLine;

namespace MOP.Terminal.CommandLineTransformers
{
    public class MiddlewareConfig : ICommandBuilderFactory
    {
        public Task<CommandLineBuilder> Transform(CommandLineBuilder builder)
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
            var isInteractive = context.ParseResult.ValueForOption<bool>("--interactive");
            if (isInteractive)
            {
                await Emulator.Execute();
                return;
            }

            await next(context);
        }
    }
}
