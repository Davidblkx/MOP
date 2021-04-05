namespace MOP.Terminal.Actors.Helpers
{
    internal static class ActorPaths
    {
        public static readonly ActorMetaData Supervisor = new(SupervisorActor.ACTOR_NAME);

        public static readonly ActorMetaData InActorMeta = new(InActor.ACTOR_NAME, Supervisor);
        public static readonly ActorMetaData EndActorMeta = new(EndActor.ACTOR_NAME, Supervisor);
        public static readonly ActorMetaData OutActorMeta = new(OutActor.ACTOR_NAME, Supervisor);
    }
}
