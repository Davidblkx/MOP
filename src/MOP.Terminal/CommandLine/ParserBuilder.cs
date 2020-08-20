using MOP.Core.Infra.Tools;
using System.Collections.Generic;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.Reflection;
using System.Threading.Tasks;

namespace MOP.Terminal.CommandLine
{
    public static class ParserBuilder
    {
        public static async Task<Parser> Build()
        {
            var builder = new CommandLineBuilder().UseDefaults();
            foreach (var transformer in GetTransformers())
            {
                builder = await transformer.Transform(builder);
            }
            return builder
                .Build();
        }

        private static IEnumerable<ICommandBuilderFactory> GetTransformers()
            => AssemblyTools.GetImplementedBy<ICommandBuilderFactory>(Assembly.GetExecutingAssembly());
    }
}
