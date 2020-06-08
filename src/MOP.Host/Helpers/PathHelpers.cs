﻿using System;
using System.IO;
using System.Runtime.InteropServices;
using static System.Environment;

namespace MOP.Host.Helpers
{
    public static class PathHelpers
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

        public static DirectoryInfo CreateIfRequired(this DirectoryInfo dir)
        {
            if (!dir.Exists) dir.Create();
            return dir;
        }

        /// <summary>
        /// Return <FileInfo> relative to current <DirectoryInfo>
        /// </summary>
        public static FileInfo RelativeFile(this DirectoryInfo directory, params string[] paths)
        {
            return new FileInfo(Path.Combine(directory.FullName, string.Join('/', paths)));
        }

        /// <summary>
        /// Return <DirectoryInfo> relative to current <DirectoryInfo>
        /// </summary>
        public static DirectoryInfo RelativeDirectory(this DirectoryInfo directory, params string[] paths)
        {
            return new DirectoryInfo(Path.Combine(directory.FullName, string.Join('/', paths)));
        }

        public static string GetNameWithoutExtension(this FileInfo file)
        {
            var extLength = file.Extension?.Length ?? 0;
            var nameLength = file.Name.Length - extLength;
            return file.Name.Substring(0, nameLength);
        }
    }
}
