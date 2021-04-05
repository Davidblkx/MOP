using System;
using System.CommandLine.Facilitator;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Threading.Tasks;

namespace MOP.Terminal.Services.Impl
{
    internal class ParserService : IParserService
    {
        private readonly Parser _parser;

        public ParserService(CommandLineFacilitator cmd)
        {
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
        }

        public Task<int> InvokeAsync(string[] args) => _parser.InvokeAsync(args);

        public Task<int> InvokeAsync(string args) => _parser.InvokeAsync(args);

        public int Invoke(string[] args) => _parser.Invoke(args);
        public int Invoke(string args) => _parser.Invoke(args);
    }
}
