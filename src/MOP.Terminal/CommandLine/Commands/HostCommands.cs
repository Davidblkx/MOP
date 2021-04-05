using MOP.Terminal.Models;
using System;
using System.CommandLine.Facilitator;

namespace MOP.Terminal.CommandLine.Commands
{
    [Command(NAME, Description = "Manage MOP host addresses")]
    internal class HostCommand
    {
        public const string NAME = "host";
    }

    [Command(
        Name = "list",
        Description = "List all available hosts",
        Aliases = new string[] { "ls" },
        Parent = typeof(HostCommand))]
    internal class HostCommandList
    {
        public const string NAME = "host_list";
    }

    [Command(
        Name = "set",
        Aliases = new string[] { "add", "update" },
        Description = "Add or Update a host",
        Parent = typeof(HostCommand))]
    internal class HostCommandSet : BaseCommand<HostCommandSet>, IHostConfig
    {
        public const string NAME = "host_set";
        [Option(Description = "replace if name already exists")]
        public bool Replace { get; set; }

        [Option(Description = "set host as default")]
        public bool IsDefault { get; set; }

        [Option(Description = "Port, defaults to 7654", Aliases = new string[] { "-p" })]
        public int Port { get; set; } = 7654;

        [Option(Description = "Hostname, defaults to localhost", Aliases = new string[] { "-h" })]
        public string Hostname { get; set; } = "localhost";

        [Option(Description = "Name to identify host", IsRequired = true)]
        public string Name { get; set; } = "";

        [Argument(Description = "Host internal ID", Arity = new int[] { 1, 1})]
        public Guid Id { get; set; }
    }

    [Command(
        Name = "remove",
        Aliases = new string[] { "rm" },
        Description = "Remove a host",
        Parent = typeof(HostCommand))]
    internal class HostCommandRemove
    {
        public const string NAME = "host_remove";
        [Option(Description = "Delete without asking for confirmation", Aliases = new string[] { "-f" })]
        public bool Force { get; set; }

        [Argument(Description = "Name to remove", Arity = new int[] { 1, 1 })]
        public string Name { get; set; } = "";
    }
}
