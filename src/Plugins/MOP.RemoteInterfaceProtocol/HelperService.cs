using MOP.Core.Domain.RIP;
using MOP.Core.Domain.RIP.Attributes;
using MOP.Core.Services;
using System.Text;

namespace MOP.RemoteInterfaceProtocol
{
    [Command("Help", Description = "List available interfaces to call from remote systems")]
    public class HelperService
    {
        private readonly IRIPService _rip;

        public HelperService(IRIPService rip)
        {
            _rip = rip;
        }

        [Action(Description = "Show all actions for a command name")]
        public string Detail(string name)
        {
            if (_rip.GetCommand(name) is not ICommand c)
                return "Can't find command for name: " + name;

            return BuildCommandDetail(c);
        }

        [Action(Description = "Show all available commands")]
        public string All()
        {
            var builder = new StringBuilder();

            foreach (var c in _rip.Commands)
                builder.Append("\n\n" + BuildCommandDetail(c));

            return builder.ToString();
        }

        private static string BuildCommandDetail(ICommand c)
        {
            return $"Command: {c.Name}";
        }
    }
}
