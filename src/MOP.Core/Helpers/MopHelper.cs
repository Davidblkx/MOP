using MOP.Core.Infra.Tools;
using Semver;
using System.Reflection;

namespace MOP.Infra.Tools
{
    public static class MopHelper
    {
        /// <summary>
        /// Gets the MOP core version.
        /// </summary>
        /// <returns></returns>
        public static SemVersion GetCoreVersion()
        {
            return AssemblyTools.GetAssemblyVersion(Assembly.GetExecutingAssembly());
        }
    }
}
