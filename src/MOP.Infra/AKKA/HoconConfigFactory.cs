using System.Collections.Generic;
using System.Text;

namespace MOP.Infra.AKKA
{
    /// <summary>
    /// Build an HOCON string using #!KEY as placeholder
    /// </summary>
    public class HoconConfigFactory
    {
        private readonly string _source;
        private readonly List<(string key, string value)> _map;

        public HoconConfigFactory(string source)
        {
            _source = source;
            _map = new List<(string, string)>();
        }

        /// <summary>
        /// Sets a value for a key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public HoconConfigFactory Set(string key, string value)
        {
            _map.Add((key, value));
            return this;
        }

        /// <summary>
        /// Builds this instance.
        /// </summary>
        /// <returns></returns>
        public string Build()
        {
            var builder = new StringBuilder(_source);
            _map.ForEach(m => builder = builder.Replace($"#!{m.key}", m.value));
            return builder.ToString();
        }
    }
}
