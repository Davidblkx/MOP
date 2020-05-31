using Semver;
using System.Diagnostics;
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
            var assembly = Assembly.GetExecutingAssembly();
            var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            return SemVersion.Parse(versionInfo.FileVersion);
        }
    }
}
