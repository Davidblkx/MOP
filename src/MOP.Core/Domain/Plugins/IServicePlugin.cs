using System;
using System.Collections.Generic;

namespace MOP.Core.Domain.Plugins
{
    /// <summary>
    /// Allow a class to be injected
    /// </summary>
    public interface IServicePlugin : IPlugin
    {
        /// <summary>
        /// Gets the service life cycle.
        /// </summary>
        /// <value>
        /// The life cycle.
        /// </value>
        public LifeCycle LifeCycle { get; }

        /// <summary>
        /// Gets the types implemented by this instance.
        /// </summary>
        public IEnumerable<Type> Implements { get; }
    }
}
