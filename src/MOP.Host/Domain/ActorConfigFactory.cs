using Akka.Configuration;
using Optional;

using static MOP.Core.Infra.Optional.Static;

namespace MOP.Host.Domain
{
    internal static class ActorConfigFactory
    {
        public static Option<Config> Build(HostProperties props)
        {
            if (!props.AllowRemote)
                return  None<Config>();

            var config = ConfigurationFactory.ParseString(BuildHOCON(props));
            return Some(config);
        }

        private static string BuildHOCON(HostProperties props)
            => GetHOCONValue()
                .Replace("#PORT", props.RemotePort.ToString())
                .Replace("#HOSTNAME", props.RemoteHostname);

        private static string GetHOCONValue()
        {
            return @"
akka {
    actor {
        provider = remote
    }
    remote {
         dot-netty.tcp {
            port = #PORT
            hostname = 0.0.0.0
            public-hostname = #HOSTNAME
        }
    }
}
";
        }
    }
}
