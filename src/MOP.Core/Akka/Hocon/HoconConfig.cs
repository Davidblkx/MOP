namespace MOP.Core.Akka.Hocon
{
    public class HoconConfig
    {
        [HoconProperty("LOG_LEVEL")]
        public LogLevel LogLevel { get; set; } = LogLevel.INFO;

        [HoconProperty("STDOUT_LOG_LEVEL")]
        public LogLevel StdoutLogLevel { get; set; } = LogLevel.OFF;

        [HoconProperty("LOG_CONFIG")]
        public Status LogConfigAtStartUp { get; set; } = Status.off;

        [HoconProperty("REMOTE_PORT")]
        public int Port { get; set; } = 7654;

        [HoconProperty("REMOTE_HOSTNAME")]
        public string Hostname { get; set; } = "0.0.0.0";

        [HoconProperty("REMOTE_PUBLIC_HOSTNAME")]
        public string PublicHostname { get; set; } = "localhost";

        [HoconProperty("REMOTE_PUBLIC_PORT")]
        public int PublicPort { get; set; } = 7654;
    }

    public enum LogLevel
    {
        OFF,
        ERROR,
        WARNING,
        INFO,
        DEBUG
    }

    public enum Status
    {
        off,
        on
    }
}
