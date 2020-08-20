using System.Text;

namespace MOP.Core.Akka.Hocon
{
    /// <summary>
    /// Build an HOCON string using #!KEY as placeholder
    /// </summary>
    public class HoconConfigFactory
    {
        private readonly string _source;
        private readonly object _config;

        public HoconConfigFactory(string source, object config)
        {
            _source = source;
            _config = config;
        }

        public HoconConfigFactory(object config)
        {
            _config = config;
            _source = HoconFileLoader.Load();
        }

        /// <summary>
        /// Builds this instance.
        /// </summary>
        /// <returns></returns>
        public string Build()
        {
            var builder = new StringBuilder(_source);
            var keyValues = HoconPropertyAttribute.GetKeyValues(_config);
            foreach (var (key, value) in keyValues)
            {
                builder.Replace(key, value);
            }
            return builder.ToString();
        }
    }
}
