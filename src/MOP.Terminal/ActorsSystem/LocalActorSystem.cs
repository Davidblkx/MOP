using MOP.Terminal.Logger;
using Akka.Actor;
using Serilog;
using System;
using MOP.Terminal.Settings;

namespace MOP.Terminal.ActorsSystem
{
    internal class LocalActorSystem
    {
        private readonly ActorSystem _system;

        public LocalActorSystem()
        {
            _system = BuildActorSystem();
        }

        private ActorSystem BuildActorSystem()
        {
            try
            {
                return ActorSystemFactory.Build(LocalSettings.Current);
            }
            catch (Exception ex)
            {
                LOG.Error(ex, "Can't start the actor system");
                throw ex;
            }
        }

        public IActorRef CreateTerminalActor(IHostSettings settings, params string[] path)
        {
            var props = TerminalActor.BuildProps(settings, path);
            return _system.ActorOf(props);
        }

        private static ILogger LOG => GlobalLogger.ForContext<LocalActorSystem>();
        private static LocalActorSystem? _systemRef;
        public static LocalActorSystem Ref
        {
            get
            {
                if (_systemRef is null)
                    _systemRef = new LocalActorSystem();
                return _systemRef;
            }
        }
    }
}
