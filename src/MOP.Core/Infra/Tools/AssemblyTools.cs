using System;
using System.Collections.Generic;
using System.Reflection;

namespace MOP.Core.Infra.Tools
{
    public static class AssemblyTools
    {
        public static MopVersion GetAssemblyVersion(Assembly assembly)
        {
            var version = assembly.
                GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                .InformationalVersion ?? "0.0.0";
            return new MopVersion(version);
        }

        /// <summary>
        /// Gets the instances from assembly, that implement <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        public static IEnumerable<T> GetImplementedBy<T>(Assembly assembly) where T : class
        {
            foreach (var t in assembly.GetTypes())
            {
                if (t.FullName is null) continue;
                if (!TypeTools.CanInstantiate<T>(t)) continue;

                if (assembly.CreateInstance(t.FullName) is T instance)
                    yield return instance;
            }
        }
    }
}
