using System.CommandLine.Facilitator;

namespace MOP.Terminal.CommandLine.Commands
{
    [Command("akka", Description = "Commands to list and change akka settings")]
    internal class AkkaCommand { }

    [Command(
        Name = "list",
        Description = "List akka settings",
        Aliases = new string[] { "ls" },
        Parent = typeof(AkkaCommand))]
    internal class AkkaCommandList
    {
        [Option(Aliases = new string[] { "-f" }, Description = "Format output")]
        public bool Format { get; set; }
    }

    [Command(
        Name = "set",
        Description = "Change akka settings",
        Parent = typeof(AkkaCommand))]
    internal class AkkaCommandSet
    {
        [Argument(
            Arity = new int[] { 1, 1},
            Description = "Key of setting to change")]
        public string Key { get; set; } = "";

        [Argument(
            Arity = new int[] { 1, 1 },
            Description = "Value to set")]
        public string Value { get; set; } = "";
    }
}
