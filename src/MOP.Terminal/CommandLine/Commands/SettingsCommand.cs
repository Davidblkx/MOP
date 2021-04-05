using System;
using System.CommandLine.Facilitator;

namespace MOP.Terminal.CommandLine.Commands
{
    [Command("settings", Description = "change and list app settings")]
    internal class SettingsCommand { }

    [Command(
        Name = "print",
        Description = "Print current settings",
        Parent = typeof(SettingsCommand))]
    internal class SettingsCommandPrint
    {
        [Option(Aliases = new string[] { "-f"}, Description = "Format output")]
        public bool Format { get; set; }

        [Option(Aliases = new string[] { "-a" }, Description = "Print also Hosts and Akka settings")]
        public bool All { get; set; }
    }

    [Command(
        Name = "set",
        Description = "Change settings",
        Parent = typeof(SettingsCommand))]
    internal class SettingsCommandSet
    {
        [Option(ArgumentName = "New UUID", Description = "Change terminal UUID, warning this could broke saved hosts")]
        public Guid? Id { get; set; }

        [Option(Description = "If active, logs are written to a log.txt file")]
        public bool? LogToFile { get; set; }
        [Option(Description = "Log level, from 0 (disabled) to 5 (verbose)")]
        public int? LogLevel { get; set; }
        [Option(Description = "The default host to use", ArgumentName ="Host name")]
        public string? DefaultHost { get; set; }
        [Option(Description = "Remove default host")]
        public bool ClearDefaultHost { get; set; }
    }
}
