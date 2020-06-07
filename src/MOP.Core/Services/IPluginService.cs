using MOP.Core.Domain.Plugins;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace MOP.Core.Services
{
    /// <summary>
    /// Load, unload plugins
    /// </summary>
    public interface IPluginService
    {
        /// <summary>
        /// Gets the loaded plugins.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IPlugin> GetLoadedPlugins();

        /// <summary>
        /// Gets the plugins waiting to be loaded.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IPlugin> GetPendingPlugins();

        /// <summary>
        /// Add plugins to pending list
        /// </summary>
        /// <param name="pluginList">The plugin list.</param>
        /// <returns></returns>
        Task AddPlugins(params IPlugin[] pluginList);

        /// <summary>
        /// Instantiate <see cref="IPlugin"/> from directory DDLs and add to pending list
        /// </summary>
        /// <param name="info">The directory information.</param>
        /// <returns></returns>
        Task AddPluginsFolder(DirectoryInfo info);

        /// <summary>
        /// Instantiate <see cref="IPlugin"/> from assembly and add to pending list
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        Task AddPluginsAssembly(Assembly assembly);

        /// <summary>
        /// Loads all pending plugins.
        /// </summary>
        /// <returns></returns>
        Task Load();
    }
}
