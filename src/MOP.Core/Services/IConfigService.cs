using Optional;
using System;
using System.Threading.Tasks;

namespace MOP.Core.Services
{
    /// <summary>
    /// Allow to create, load and save objects, it needs to be serializable
    /// </summary>
    public interface IConfigService
    {
        /// <summary>
        /// Gets the configuration for the specified identifier.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">The identifier.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public Task<Option<T>> LoadConfig<T>(Guid id, T defaultValue = default);

        /// <summary>
        /// Saves the configuration.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">The identifier.</param>
        /// <param name="value">The value.</param>
        /// <returns>True, if successfully saved</returns>
        public Task<bool> SaveConfig<T>(Guid id, T value);
    }
}
