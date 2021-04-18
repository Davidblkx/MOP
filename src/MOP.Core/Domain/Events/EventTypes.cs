namespace MOP.Core.Domain.Events
{
    public static class EventTypes
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
    }
}
