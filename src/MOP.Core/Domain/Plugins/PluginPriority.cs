namespace MOP.Core.Domain.Plugins
{
    public static class PluginPriority
    {
        /// <summary>
        /// Plugins that required to be the first ones to load
        /// </summary>
        public const ulong CORE = 10;

        /// <summary>
        /// Default load order
        /// </summary>
        public const ulong DEFAULT = 10000;

        /// <summary>
        /// The last plugins to be loaded
        /// </summary>
        public const ulong LOW = ulong.MaxValue;
    }
}
