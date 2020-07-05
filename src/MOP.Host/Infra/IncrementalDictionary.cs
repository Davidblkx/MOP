
using Optional;
using System;
using System.Collections.Generic;

using static MOP.Core.Helpers.NullHelper;

namespace MOP.Host.Infra
{
    /// <summary>
    /// Contains a collection of <typeparamref name="T"/> assigned to a id
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class IncrementalDictionary<T> where T : notnull
    {
        /// <summary>
        /// Gets the next identifier to be inserted.
        /// </summary>
        /// <value>
        /// The next identifier.
        /// </value>
        public ulong NextId { get; private set; }

        /// <summary>
        /// Gets or sets the value transformer.
        /// 
        /// The transformer allows to unify the values, for instance, 
        /// if T is string we could do a ToUpperCase(), so the values would not be case sensitive
        /// </summary>
        /// <value>
        /// The transformer.
        /// </value>
        public Func<T, T>? Transformer { get; set; }

        private readonly Dictionary<T, ulong> _values;
        private readonly Dictionary<ulong, T> _keys;

        public IncrementalDictionary()
        {
            _values = new Dictionary<T, ulong>();
            _keys = new Dictionary<ulong, T>();
            NextId = 0;
        }

        /// <summary>
        /// try to get the id for a key, if not found 
        /// value is added and the new id returned
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The id value</returns>
        public ulong GetId(T value)
        {
            T val = GetValue(value);
            if (_values.TryGetValue(val, out var id))
                return id;

            return AddNewValue(val);
        }

        /// <summary>
        /// Gets the value for an id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public Option<T> GetValue(ulong id)
        {
            if (_keys.TryGetValue(id, out var value))
                return Some(value);
            return None<T>();
        }

        /// <summary>
        /// Adds the new value, return the id and increments the next id
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private ulong AddNewValue(T value)
        {
            _values.Add(value, NextId);
            _keys.Add(NextId, value);
            return NextId++;
        }

        private T GetValue(T value)
            => Transformer is null ? value : Transformer(value);
    }

    public static class IncrementalDictionary
    {
        public static IncrementalDictionary<T> Create<T>(Func<T, T> transformer)
            where T : notnull
        {
            return new IncrementalDictionary<T>
                { Transformer = transformer };
        }
    }
}
