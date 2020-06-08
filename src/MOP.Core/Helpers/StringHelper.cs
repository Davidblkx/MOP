using Optional;
using System.IO;

namespace MOP.Core.Helpers
{
    public static class StringHelper
    {
        /// <summary>
        /// return the value, or if it is null or empty, return the defaultValue
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static string ValueOrDefault(this string? value, string defaultValue)
        {
            if (value is null || value.Length == 0)
                return defaultValue;
            return value;
        }

        /// <summary>
        /// Converts to optional, if is null or empty return None
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Option<string> ToOptional(this string? value)
        {
            if (value is null || value.Length == 0)
                return Option.None<string>();
            return Option.Some(value);
        }

        /// <summary>
        /// Converts to <see cref="FileInfo"/>, if is not a valid path, return none.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Option<FileInfo> ToFileInfo(this string? value)
        {
            try { return Option.Some(new FileInfo(value)); }
            catch { return Option.None<FileInfo>();  }
        }
    }
}
