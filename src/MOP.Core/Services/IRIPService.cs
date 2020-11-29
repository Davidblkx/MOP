using MOP.Core.Domain.RIP;
using System;
using System.Collections.Generic;

namespace MOP.Core.Services
{
    /// <summary>
    /// Remote interface protocol service
    /// </summary>
    public interface IRIPService
    {
        /// <summary>
        /// Gets the available commands.
        /// </summary>
        /// <value>
        /// The commands.
        /// </value>
        IEnumerable<ICommand> Commands { get; }

        /// <summary>
        /// Registers a new command.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="replace">if set to <c>true</c> [replace].</param>
        /// <returns><c>True</c> if command was created and registered.
        /// If <c>False</c> type does not implement the CommandAttribute or 
        /// Name is already in use and replace is false</returns>
        bool Register<T>(bool replace = false);

        /// <summary>
        /// Registers a new command.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="replace">if set to <c>true</c> [replace].</param>
        /// <returns><c>True</c> if command was created and registered.
        /// If <c>False</c> type does not implement the CommandAttribute or 
        /// Name is already in use and replace is false</returns>
        bool Register(Type type, bool replace = false);

        /// <summary>
        /// Determines whether the specified name was used.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        ///   <c>true</c> if the specified name has name; otherwise, <c>false</c>.
        /// </returns>
        bool HasName(string name);

        /// <summary>
        /// Gets the type of the command.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>null if name is not found</returns>
        Type? GetCommandType(string name);

        /// <summary>
        /// Gets the command.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        ICommand? GetCommand(string name);
    }
}
