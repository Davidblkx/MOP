using Akka.Actor;
using MOP.Terminal.Models;
using MOP.Terminal.Services;
using System;
using System.CommandLine;
using System.Threading.Tasks;

using static MOP.Terminal.Infra.DependencyInjector;

namespace MOP.Terminal.Actors
{
    abstract class InternalBaseCommandActor<T> : ReceiveActor, ICommandActor
    {
        protected static StartupArgs StartupArgs => GetInstance<StartupArgs>();
        protected static ICommandLineService CMD => GetInstance<ICommandLineService>();

        public InternalBaseCommandActor()
        {
            ReceiveAsync<T>(OnReceiveCommand);
        }

        protected virtual async Task OnReceiveCommand(T command)
        {
            var result = await HandleCommand(command);
            Next(result);
        }

        protected abstract Task<int> HandleCommand(T command);

        protected void Next(int result)
        {
            AppState.Result = result;
            if (!StartupArgs.Interactive)
                AppState.Life.Cancel();
            else HandleNewCommand();
        }

        protected void HandleNewCommand()
        {
            Console.Write("\n> ");
            var cmd = Console.ReadLine();
            if (cmd == "exit") { AppState.Life.Cancel(); return; };
            if (cmd == "clear") { Console.Clear(); HandleNewCommand(); }
            if (cmd is null) HandleNewCommand();
            else CMD.RootCommand.InvokeAsync(cmd);
        }

        protected static void Write(string text)
            => Spectre.Console.AnsiConsole.Write(text);

        protected static void WriteLine(string text)
            => Spectre.Console.AnsiConsole.WriteLine(text);
    }

    interface ICommandActor { }
}
