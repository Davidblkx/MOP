using System.Collections.Generic;
using System.CommandLine.Facilitator;

namespace MOP.Terminal.CommandLine.Commands
{
    [Command("send", Description = "Send command to remote host")]
    internal class SendCommand
    {
        [Option(
            ArgumentName = "Host Name",
            Aliases = new string[] { "-n" },
            Description = "Name of the host to use")]
        public string? HostName { get; set; }

        [Argument(Arity = new int[] { 1, 1 }, Description = "Command to use")]
        public string Command { get; set; } = "";

        [Argument(Arity = new int[] { 1, 1 }, Description = "Action to invoke")]
        public string Action { get; set; } = "";

        [Argument(Description = "Action parameters in JSON format")]
        public IEnumerable<string> Parameters { get; set; } = new List<string>();
    }
}
