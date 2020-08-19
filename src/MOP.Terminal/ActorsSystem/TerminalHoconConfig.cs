namespace MOP.Terminal.ActorsSystem
{
    internal class TerminalHoconConfig
    {
        public const string CONFIG = @"
akka {
    actor {
        provider = remote
    }
    remote {
         dot-netty.tcp {
            port = #!PORT
            hostname = 0.0.0.0
            public-hostname = #!HOSTNAME
        }
    }
}
";
    }
}
