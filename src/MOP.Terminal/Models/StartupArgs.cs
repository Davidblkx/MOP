namespace MOP.Terminal.Models
{
    /// <summary>
    /// The startup arguments, they are global and can be passed at any position
    /// </summary>
    public record StartupArgs(bool Interactive, string SettingsFile);

    /// <summary>
    /// Names of the arguments to start this app
    /// </summary>
    public static class StartupArgName
    {
        public const string Interactive = "--interactive";
        public const string InteractiveShort = "-i";
        public const string SettingsFile = "--settings";
    }
}
