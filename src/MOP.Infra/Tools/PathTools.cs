using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

using static System.Environment;

namespace MOP.Infra.Tools
{
    public static class PathTools
    {
        /// <summary>
        /// return Windows %APPDATA%
        /// </summary>
        /// <returns></returns>
        public static DirectoryInfo GetWindowsAppData()
        {
            return new DirectoryInfo(
                GetFolderPath(SpecialFolder.ApplicationData));
        }

        public static DirectoryInfo GetUnixHomeDir()
        {
            return new DirectoryInfo(
                GetFolderPath(SpecialFolder.UserProfile));
        }

        public static DirectoryInfo GetUserHome()
            => RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ?
                GetWindowsAppData() : GetUnixHomeDir();

        /// <summary>
        /// Gets the start directory.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="AccessViolationException">Can't locate running assembly location</exception>
        public static DirectoryInfo GetStartDirectory()
        {
            var location = Process.GetCurrentProcess().MainModule.FileName;
            var file = new FileInfo(location);
            if (!file.Exists)
            {
                throw new AccessViolationException("Can't locate running assembly location");
            }

            return file.Directory;
        }
    }
}
