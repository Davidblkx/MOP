using MOP.Core.Domain.Plugins;
using System;
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
        IEnumerable<DirectoryInfo> PluginFolders { get; }

        /// <summary>
        /// Gets the loaded plugins.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IPlugin> GetLoadedPlugins();

        /// <summary>
        /// Gets the plugins that failed to load.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IPlugin> GetFailedPlugins();
        /// <summary>
        /// Types waiting to be instantiated
        /// </summary>
        /// <returns></returns>
        IEnumerable<Type> GetPending();

        /// <summary>
        /// Add plugins to pending list
        /// </summary>
        /// <param name="pluginList">The plugin list.</param>
        /// <returns></returns>
        void AddPlugins(params Type[] pluginList);

        /// <summary>
        /// Instantiate <see cref="IPlugin"/> from directory DDLs and add to pending list
        /// </summary>
        /// <param name="info">The directory information.</param>
        /// <returns></returns>
        void AddPluginsFolder(DirectoryInfo info);

        /// <summary>
        /// Instantiate <see cref="IPlugin"/> from assembly and add to pending list
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        void AddPluginsAssembly(Assembly assembly);

        /// <summary>
        /// Loads all pending plugins.
        /// </summary>
        /// <returns></returns>
        Task Load();
    }
}
