using MOP.Core.Domain.Plugins;
using Optional;
using System;
using System.Reflection;

namespace MOP.Core.Infra.Tools
{
    public static class TypeTools
    {
        /// <summary>
        /// Checks if <paramref name="target" /> is non abstract class and implements <typeparamref name="T"/>.
        /// if <paramref name="target" /> equals <typeparamref name="T"/> is returned false
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">The target.</param>
        /// <returns>
        ///   <c>true</c> if this instance type can be instantiate as <typeparamref name="T"/>; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentException">target base must be an interface</exception>
        public static bool CanInstantiate<T>(Type target)
            => CanInstantiate(target, typeof(T));

        /// <summary>
        /// Checks if <paramref name="target" /> is non abstract class and implements <paramref name="targetBase" />.
        /// if <paramref name="target" /> equals <paramref name="targetBase" /> is returned false
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="targetBase">The target base.</param>
        /// <returns>
        ///   <c>true</c> if this instance type can be instantiate as targetBase; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentException">target base must be an interface</exception>
        public static bool CanInstantiate(Type target, Type targetBase)
        {
            if (!targetBase.IsInterface)
                throw new ArgumentException("target base must be an interface");

            if (targetBase.FullName == target.FullName)
                return false;

            var implements = !(target.GetInterface(targetBase.Name) is null);
            var hasName = !(target.FullName is null);

            return !target.IsAbstract
                && !target.IsInterface
                && implements
                && hasName;
        }

        /// <summary>
        /// Check if type should the be ignored.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static bool ShouldBeIgnored(Type type)
            => !(type.GetCustomAttribute<IgnorePluginAttribute>() is null);
    }
}
