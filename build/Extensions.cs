

using Nuke.Common.Tools.DotNet;

namespace MOPBUILD
{
    static class Extensions
    {
        public static DotNetBuildSettings SetMopRuntime(this DotNetBuildSettings s, string runtime)
        {
            return string.IsNullOrEmpty(runtime) ?
                s : s.SetRuntime(runtime);
        }

        public static DotNetRestoreSettings SetMopRuntime(this DotNetRestoreSettings s, string runtime)
        {
            return string.IsNullOrEmpty(runtime) ?
                s : s.SetRuntime(runtime);
        }

        public static DotNetPublishSettings SetMopRuntime(this DotNetPublishSettings s, string runtime)
        {
            return string.IsNullOrEmpty(runtime) ?
                s : s.SetRuntime(runtime);
        }
    }

}