using System.Collections.Generic;

namespace MOP.DirectoryWatcher.Models
{
    public class DirectoryWatchConfig
    {
        /// <summary>
        /// Directories to watch/scan
        /// </summary>
        public List<FolderToWatch> Directories { get; set; } = new();
        /// <summary>
        /// Start watching for changes
        /// </summary>
        public bool WatchForChanges { get; set; }
        /// <summary>
        /// Scan all directories on plugin start
        /// </summary>
        public bool ScanOnStart { get; set; }
        /// <summary>
        /// Directories scanned in parallel
        /// </summary>
        public int ScanThreads { get; set; } = 10;
    }
}
