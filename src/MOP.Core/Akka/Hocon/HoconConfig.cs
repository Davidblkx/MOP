using Akka.Actor;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace MOP.Core.Akka.Hocon
{
    public class HoconConfig
    {
        [HoconProperty("LOG_LEVEL")]
        public LogLevel LogLevel { get; set; } = LogLevel.INFO;

        [HoconProperty("STDOUT_LOG_LEVEL")]
        public LogLevel StdoutLogLevel { get; set; } = LogLevel.INFO;

        [HoconProperty("LOG_CONFIG")]
        public Status LogConfigAtStartUp { get; set; } = Status.off;

        [HoconProperty("REMOTE_PORT")]
        public int Port { get; set; } = 7654;

        [HoconProperty("REMOTE_HOSTNAME")]
        public string Hostname { get; set; } = "localhost";

        [HoconProperty("REMOTE_PUBLIC_HOSTNAME")]
        public string PublicHostname { get; set; } = "localhost";

        [HoconProperty("REMOTE_PUBLIC_PORT")]
        public int PublicPort { get; set; } = 7654;

        [JsonIgnore]
        [HoconProperty("CLUSTER_SEED_NODES")]
        public string ComputedClusterSeedNodes => ComputeSeedNodes();

        [JsonIgnore]
        [HoconProperty("CLUSTER_ROLES")]
        public string ComputedClusterRoles => CommaSeparate(ClusterRoles);

        public List<string> ClusterSeedNodes { get; set; } = new();
        public List<string> ClusterRoles { get; set; } = new();

        public string ActorSystemName { get; set; } = "mop";

        /// <summary>
        /// If role is not presented, is added
        /// </summary>
        /// <param name="value"></param>
        public void EnsureRole(string value)
        {
            if (!ClusterRoles.Contains(value))
                ClusterRoles.Add(value);
        }

        private string ComputeSeedNodes()
        {
            var self = new Address("akka.tcp", ActorSystemName, PublicHostname, PublicPort);
            var seeds = new List<string> { self.ToString() };
            seeds.AddRange(ClusterSeedNodes);

            return CommaSeparate(seeds);
        }

        private string CommaSeparate(IEnumerable<string> list)
            => string.Join(",", list.Select(e => $"\"{e}\""));
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
