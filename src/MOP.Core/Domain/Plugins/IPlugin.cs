using MOP.Infra.Domain.Host;
using System;
using System.Threading.Tasks;

namespace MOP.Infra.Domain.Plugins
{
    /// <summary>
    /// To be implemented by plugins
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IPlugin : IDisposable
    {
        /// <summary>
        /// Gets the information.
        /// </summary>
        /// <value>
        /// The information.
        /// </value>
        IPluginInfo Info { get; }

        /// <summary>
        /// Initializes plugin
        /// </summary>
        /// <returns></returns>
        Task<bool> Initialize();
    }
}
