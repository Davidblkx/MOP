namespace MOP.Core.Domain.Events
{
    public static class EventTypes
    {
        /// <summary>
        /// Events for IO operations
        /// </summary>
        public static class IO
        {
            /// <summary>
            /// Add directory to watch list so changes are tracked
            /// </summary>
            public static string AddWatchDirectory => "CORE_DIRECTORY_ADD_WATCH";

            /// <summary>
            /// Remove directory from watch list
            /// </summary>
            public static string RemoveWatchDirectory => "CORE_DIRECTORY_ADD_WATCH";

            /// <summary>
            /// A file was changed in a directory
            /// </summary>
            public static string DirectoryFileChanged => "CORE_DIRECTORY_FILE_CHANGED";

            /// <summary>
            /// A file was scanned in a directory
            /// </summary>
            public static string DirectoryFileFound => "CORE_DIRECTORY_FILE_FOUND";

            /// <summary>
            /// A file was added to a directory
            /// </summary>
            public static string DirectoryFileAdded => "CORE_DIRECTORY_FILE_CHANGED";

            /// <summary>
            /// A file was removed from a directory
            /// </summary>
            public static string DirectoryFileRemoved => "CORE_DIRECTORY_FILE_CHANGED";

            /// <summary>
            /// A file was moved in a directory
            /// </summary>
            public static string DirectoryFileMoved => "CORE_DIRECTORY_FILE_CHANGED";

            /// <summary>
            /// Scan all files in directory
            /// </summary>
            public static string DirectoryScan => "CORE_DIRECTORY_SCAN";
        }

        /// <summary>
        /// Events for progress report
        /// </summary>
        public static class Progress
        {
            /// <summary>
            /// A new task with progress report started
            /// </summary>
            public static string TaskStarted => "CORE_PROGRESS_TASK_STARTED";

            /// <summary>
            /// A task was completed
            /// </summary>
            public static string TaskEnded => "CORE_PROGRESS_TASK_END";

            /// <summary>
            /// Report for a task in progress
            /// </summary>
            public static string ProgressReport => "CORE_PROGRESS_REPORT";
        }
    }
}
