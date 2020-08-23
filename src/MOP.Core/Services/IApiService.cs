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
        /// Gets the by path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        Option<ActorInstance> GetByPath(string path);
        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        IEnumerable<ActorInstance> GetAll();
        /// <summary>
        /// Adds the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        void Add(ActorInstance instance);
        /// <summary>
        /// Adds the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        void Add(Type type);
        /// <summary>
        /// Adds the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        void Add(Assembly assembly);
    }
}
