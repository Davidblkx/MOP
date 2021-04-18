using System.Collections.Generic;

namespace MOP.DirectoryWatcher.Models
{
    public class DirectoryWatchConfig
    {
        public List<FolderToWatch> Directories { get; set; } = new();
    }
}
