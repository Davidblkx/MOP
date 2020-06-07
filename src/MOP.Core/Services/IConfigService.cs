using MOP.Core.Domain.Configs;
using Optional;
using System;
using System.Threading.Tasks;

namespace MOP.Core.Services
{
    /// <summary>
    /// Allow to create, load and save <see cref="IConfigStore"/>
    /// </summary>
    public interface IConfigService
    {
        /// <summary>
        /// Loads a configuration store
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<Option<IConfigStore>> GetStore(Guid id);

        /// <summary>
        /// Creates the store.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<Option<IConfigStore>> CreateStore(Guid id);

        /// <summary>
        /// Saves the store.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <returns><c>true</c>, if save was successfully</returns>
        Task<bool> SaveStore(IConfigStore store);

        /// <summary>
        /// Determines whether the specified unique identifier has store.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        Task<bool> HasStore(Guid id);

        /// <summary>
        /// Removes the store.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<bool> RemoveStore(Guid id);
    }
}
