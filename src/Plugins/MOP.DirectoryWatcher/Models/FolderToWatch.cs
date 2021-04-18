using System;

namespace MOP.DirectoryWatcher.Models
{
    public class FolderToWatch
    {
        public string FolderPath { get; set; } = "";
        public DateTime Added { get; set; }
    }
}
