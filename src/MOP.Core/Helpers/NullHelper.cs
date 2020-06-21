﻿using Optional;
using System;

namespace MOP.Core.Helpers
{
    public static class NullHelper
    {
        /// <summary>
        /// Throws on null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static T ThrowOnNull<T>(T? value)
            where T : class
        {
            if (value is null)
                throw new ArgumentNullException($"{typeof(T)} is null");

            return value;
        }

        /// <summary>
        /// Same as Optional.Some, but null is always None
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Option<T> Some<T>(T value)
            => value is null ? 
                Option.None<T>() 
                : Option.Some(value);

        public static Option<T> None<T>()
            => Option.None<T>();
    }
}
