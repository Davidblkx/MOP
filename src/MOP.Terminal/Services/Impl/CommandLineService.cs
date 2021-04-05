using MOP.Terminal.Models;
using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Facilitator;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Threading.Tasks;

namespace MOP.Terminal.Services
{
    internal class CommandLineService : ICommandLineService
    {
        private readonly StartupArgs _startupArgs;
        private readonly RootCommand _root;
        private readonly Parser _parser;

        public RootCommand RootCommand => _root;

        public CommandLineService(StartupArgs args, CommandLineFacilitator cmd)
        {
            _startupArgs = args;
            _parser = cmd
                .GetCommandLineBuilder()
                .UseVersionOption()
                .UseHelp()
                .UseEnvironmentVariableDirective()
                .UseParseDirective()
                .UseDebugDirective()
                .UseSuggestDirective()
                .RegisterWithDotnetSuggest()
                .UseTypoCorrections()
                .UseExceptionHandler()
                .CancelOnProcessTermination()
                .UseMiddleware(async (context, next) =>
                {
                    if (context.ParseResult.Errors.Count > 0)
                    {
                        // TODO: EMIT ERROR MESSAGE
                        context.InvocationResult = new ParseErrorResult();
                    }
                    else
                    {
                        await next(context);
                    }
                }, MiddlewareOrder.ErrorReporting)
                .Build();
            _root = new();
        }

        public Task<int> Invoke(string[] args)
        {
            if (!_startupArgs.Interactive)
                return _parser.InvokeAsync(args);

            return EmulateTerminal();
        }

        private async Task<int> EmulateTerminal()
        {
            while (true)
            {
                Console.Write("\n> ");
                var cmd = Console.ReadLine();
                if (cmd == "exit") break;
                if (cmd == "clear") { Console.Clear(); continue; }
                if (cmd is null) continue;
                await _parser.InvokeAsync(cmd);
            }

            return 0;
        }
    }
}
