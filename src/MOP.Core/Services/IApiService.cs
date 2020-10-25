using MOP.Core.Domain.Api;
using Optional;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace MOP.Core.Services
{
    /// <summary>
    /// Allow to add and read API description for actors
    /// </summary>
    public interface IApiService
    {
        /// <summary>
        /// Gets the API by name or path
        /// </summary>
        /// <param name="nameOrPath">The name or path.</param>
        /// <returns></returns>
        Option<ApiHost> GetByPathOrName(string nameOrPath);

        /// <summary>
        /// Gets all saved APIs.
        /// </summary>
        /// <returns></returns>
        IEnumerable<ApiHost> GetAll();

        /// <summary>
        /// Adds the specified API.
        /// </summary>
        /// <param name="api">The API.</param>
        void Add(ApiHost api);

        /// <summary>
        /// Load ApiHost from type
        /// </summary>
        /// <param name="type">The type.</param>
        void Add(Type type);

        /// <summary>
        /// Load all ApiHost from assembly
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        void Add(Assembly assembly);
    }
}
