using MOP.Core.Akka;
using System;
using System.IO;

namespace MOP.DirectoryWatcher.Models
{
    public class ScanDirectoryResults : IPriorityMessage
    {
        public Guid TaskId { get; set; }
        public int Diretories { get; set; }

        public int Priority => 0;
    }

    public class ScanDirectory : IPriorityMessage
    {
        public Guid TaskId { get; set; }
        public DirectoryInfo? Directory { get; set; }

        public int Priority => 0;
    }
}
