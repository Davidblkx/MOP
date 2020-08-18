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
        /// Executed before plugins are initialized.
        /// </summary>
        /// <param name="IHost">The i host.</param>
        /// <returns></returns>
        Task PreLoad(IHost host);

        /// <summary>
        /// Initializes plugin using the specified host.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <returns></returns>
        Task<bool> Initialize();
    }
}
