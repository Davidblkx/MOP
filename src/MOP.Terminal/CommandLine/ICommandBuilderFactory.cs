using System.CommandLine.Builder;
using System.Threading.Tasks;

namespace MOP.Terminal.CommandLine
{
    public interface ICommandBuilderFactory
    {
        Task<CommandLineBuilder> Transform(CommandLineBuilder builder);
    }
}
