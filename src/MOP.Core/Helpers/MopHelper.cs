using Semver;
using System.Reflection;

namespace MOP.Core.Helpers
{
    public static class MopHelper
    {
        /// <summary>
        /// Gets the MOP core version.
        /// </summary>
        /// <returns></returns>
        public static SemVersion GetCoreVersion()
        {
            var versionInfo = Assembly.GetExecutingAssembly()
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                .InformationalVersion;
            return SemVersion.Parse(versionInfo);
        }
    }
}
