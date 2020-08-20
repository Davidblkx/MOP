using System;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using System.IO;
using MOP.Terminal.Settings;
using MOP.Terminal.ConsoleEmulator;
using MOP.Terminal.Logger;
using MOP.Terminal.CommandLine;
using MOP.Core.Infra.Extensions;

namespace MOP.Terminal.CommandLineTransformers
{
    public class MiddlewareConfig : ICommandBuilderFactory
    {
        public Task<CommandLineBuilder> Transform(CommandLineBuilder builder)
        {
            return Task.Run(() => builder
                .UseMiddleware(LoadSettingsMiddleware, MiddlewareOrder.Configuration)
                .UseMiddleware(LoadVerboseMiddleware, MiddlewareOrder.Configuration + 1)
                .UseMiddleware(DefaultCommandMiddleware, MiddlewareOrder.ErrorReporting - 1)
            );
        }

        public async Task LoadSettingsMiddleware(
            InvocationContext context, Func<InvocationContext, Task> next)
        {
            var val = context.ParseResult.ValueForOption<string>("--settings");
            var reload = context.ParseResult.ValueForOption<bool>("--reload-settings");

            if (!reload && LocalSettings.HasInit)
            {
                await next(context);
                return;
            }

            if (!val.IsNullOrEmpty())
            {
                var file = new FileInfo(val);
                SettingsHandler.ReloadInstance(file);
                return;
            }

            await LocalSettings.ReloadSettings();
            await next(context);
        }

        public async Task LoadVerboseMiddleware(
            InvocationContext context, Func<InvocationContext, Task> next)
        {
            var val = context.ParseResult.ValueForOption<int>("--verbose");
            if (val >= 0 && val <= 5)
            {
                LocalSettings.Current.LogLevel = val;
            } else if (val > 5)
            {
                GlobalLogger.Log.Warning("Verbose level can't be higher than 5");
            }

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
