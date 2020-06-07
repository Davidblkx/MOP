using Optional;
using System;

namespace MOP.Core.Domain.Configs
{
    /// <summary>
    /// Allow to save/load configuration using a key/value format
    /// </summary>
    public interface IConfigStore
    {
        /// <summary>
        /// Gets the store identifier.
        /// </summary>
        /// <value>
        /// The store identifier.
        /// </value>
        Guid StoreId { get; }

        /// <summary>
        /// Sets the value for a key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="replace">if set to <c>true</c> [replace].</param>
        /// <returns>true, if value was saved</returns>
        bool SetValue<T>(string key, T value, bool replace = true);

        /// <summary>
        /// Gets the value, if not present save and return default
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        Option<T> GetValue<T>(string key, T defaultValue);

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        Option<T> GetValue<T>(string key);

        /// <summary>
        /// Determines whether the specified key has value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   <c>true</c> if the specified key has value; otherwise, <c>false</c>.
        /// </returns>
        bool HasValue<T>(string key);
    }
}
