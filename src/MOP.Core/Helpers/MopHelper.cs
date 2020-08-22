using MOP.Core.Infra;
using MOP.Core.Infra.Tools;
using System.Reflection;

namespace MOP.Infra.Tools
{
    public static class MopHelper
    {
        /// <summary>
        /// Gets the MOP core version.
        /// </summary>
        /// <returns></returns>
        public static MopVersion GetCoreVersion()
        {
            return AssemblyTools.GetAssemblyVersion(Assembly.GetExecutingAssembly());
        }
    }
}
