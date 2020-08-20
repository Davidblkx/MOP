using MOP.Terminal.CommandLine;
using MOP.Terminal.Logger;
using MOP.Terminal.Settings;
using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.Threading.Tasks;
using System.CommandLine.Invocation;
using MOP.Core.Infra.Extensions;

namespace MOP.Terminal.CommandLineTransformers
{
    public class SetCommand : ICommandBuilderFactory
    {
        public Task<CommandLineBuilder> Transform(CommandLineBuilder builder)
            => Task.Run(() => builder.AddCommand(BuildSetCommand()));

        private Option<bool> BuildSaveOption()
            => new Option<bool>("--save", "Save change to settings file");

        private Command BuildSetCommand()
        {
            var cmd = new Command("set")
            {
                Description = "Add or update a setting value",
                Handler = CommandHandler.Create(
                    (bool save, string key, string value) 
                        => ChangeSettings(save, key, value)
                )
            };
            cmd.AddOption(BuildSaveOption());
            cmd.AddArgument(new Argument("key") 
                { 
                    Description = "Settings key",
                    Arity = ArgumentArity.ExactlyOne
                });
            cmd.AddArgument(new Argument("value")
            {
                Description = "Settings value",
                Arity = ArgumentArity.ExactlyOne
            });

            return cmd;
        }

        private async Task ChangeSettings(bool save, string key, string value)
        {
            var log = GlobalLogger.ForContext<SetCommand>();
            try
            {
                LocalSettings.Current.GetType()
                    .SetValue(LocalSettings.Current, key, value);

                if (save)
                    await LocalSettings.SaveSettings();

                GlobalLogger.InitLogger();

                log.Information("Settings updated");
                log.Debug("Updated {@Key} with value: {@Value}", key, value);
            } 
            catch (Exception ex)
            {
                log.Error(ex, "Error updating {@Key} with value: {@Value}", key, value);
            }
        }
    }
}
