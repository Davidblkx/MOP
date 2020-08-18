namespace MOP.Infra.Model
{
    public static class MopActorNames
    {
        public static class Metadata {
            /// <summary>
            /// Creates and supervises metadata actors
            /// </summary>
            public const string Main = "MOP_METADATA_MAIN";

            /// <summary>
            /// Handle commands for metadata, and generate events
            /// </summary>
            public const string Handler = "MOP_METADATA_HANDLER";

            /// <summary>
            /// Store all events
            /// </summary>
            public const string EventStore = "MOP_METADATA_EVENT_STORE";
        }
    }
}