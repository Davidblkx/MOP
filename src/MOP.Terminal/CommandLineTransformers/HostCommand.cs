using ConsoleInteractive;
using MOP.Terminal.CommandLine;
using MOP.Terminal.Logger;
using MOP.Terminal.Settings;
using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace MOP.Terminal.CommandLineTransformers
{
    public class HostCommand : ICommandBuilderFactory
    {
        public Task<CommandLineBuilder> Transform(CommandLineBuilder builder)
        {
            return Task.Run(() => builder.AddCommand(BuildHostCommand()));
        }

        private Command BuildHostCommand()
        {
            var cmd = new Command("host", "Manage hosts");
            cmd.AddCommand(BuildListCommand());
            cmd.AddCommand(BuildAddCommand());
            cmd.AddCommand(BuildRemoveCommand());
            return cmd;
        }

        private Command BuildListCommand()
        {
            var cmd = new Command("list", "List available hosts");
            cmd.AddAlias("ls");
            cmd.Handler = CommandHandler.Create(ListCommands);
            return cmd;
        }

        private Command BuildAddCommand()
        {
            var cmd = new Command("set", "Add or update a host");
            cmd.AddAlias("add");
            cmd.AddAlias("update");
            cmd.AddOption(new Option<bool>("--replace", "replace host id ID already exists"));
            cmd.AddOption(new Option<bool>("--set-default", "set as default host to use"));
            cmd.AddArgument(new Argument<string>("name", "Friendly name for host") { Arity = ArgumentArity.ExactlyOne });
            cmd.AddArgument(new Argument<string>("hostname", "host domain: IP, localhost or address") { Arity = ArgumentArity.ExactlyOne });
            cmd.AddArgument(new Argument<int>("port", "port to use") { Arity = ArgumentArity.ExactlyOne });
            cmd.AddArgument(new Argument<Guid>("id", "host identifier") { Arity = ArgumentArity.ExactlyOne });
            cmd.Handler = CommandHandler.Create(
                (string name, string hostname, int port, Guid id, bool replace, bool setDefault) => 
                    AddHost(name, hostname, port, id, replace, setDefault));
            return cmd;
        }

        private Command BuildRemoveCommand()
        {
            var cmd = new Command("remove", "remove a host by name");
            cmd.AddOption(new Option<bool>(new string[] { "--force", "-f" }, "don't ask for confirmation"));
            cmd.AddArgument(new Argument<string>("name", "Friendly name for host") { Arity = ArgumentArity.ExactlyOne });
            cmd.Handler = CommandHandler.Create((string name, bool force) => RemoveHost(name, force));
            return cmd;
        }

        private void ListCommands()
        {
            foreach (var h in LocalSettings.Current.Hosts)
            {
                Console.WriteLine($"{h.Name}=>{h.Id}@{h.Hostname}:{h.Port}");
            }
        }

        private void RemoveHost(string name, bool force)
        {
            if (!FindHost(name)) return;
            var message = $"Do you want to remove host {name}";
            if (force || ConsoleI.AskConfirmation(message))
            {
                LocalSettings.Current.Hosts.RemoveAll(e => e.Name == name);
                GlobalLogger.Log.Information($"Host [{name}] was removed!");
            }
        }

        private async Task AddHost(string name, string hostname, int port, Guid id, bool replace, bool setDefault)
        {
            var log = GlobalLogger.Log;
            if (!replace && FindHost(name))
            {
                log.Error($"Host [{name}] already exists use --replace to update");
                return;
            }

            AddHost(name, hostname, port, id);
            var hostList = LocalSettings.Current.Hosts;
            if (setDefault || hostList.Count == 1)
            {
                LocalSettings.Current.DefaultHost = name;
                log.Information("Host {@Name} set as default", name);
            }

            await LocalSettings.SaveSettings();
            log.Information("Host saved!");
        }

        private void AddHost(string name, string hostname, int port, Guid id)
        {
            var host = HostSettings.From(name, hostname, port, id);
            var hostList = LocalSettings.Current.Hosts;
            hostList.RemoveAll(e => e.Name == name);
            hostList.Add(host);
            LocalSettings.Current.Hosts = hostList;
            GlobalLogger.Log.Debug($"Added host: {id}");
        }

        private bool FindHost(string name)
        {
            return LocalSettings
                .Current.Hosts
                .Find(e => e.Name == name) != null;
        }
    }
}
