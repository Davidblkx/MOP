using System.CommandLine.Builder;
using System.Threading.Tasks;

namespace MOP.Terminal.Commands
{
    public interface ICommandConfig
    {
        Task<CommandLineBuilder> ApplyConfig(CommandLineBuilder builder);
    }
}
