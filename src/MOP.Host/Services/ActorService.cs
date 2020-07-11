using Akka.Actor;
using MOP.Core.Domain.Actors;
using MOP.Core.Domain.Host;
using MOP.Core.Services;
using Optional;
using Optional.Unsafe;
using Serilog;
using System;
using System.Collections.Generic;

using static MOP.Core.Optional.StaticOption;

namespace MOP.Host.Services
{
    internal class ActorService : IActorService
    {
        private readonly IHost _host;
        private readonly Dictionary<string, IActorFactory> _factories;
        private readonly Dictionary<string, IActorRef> _instances;
        private readonly ILogger _log;
        public ActorSystem MainActorSystem { get; }

        public ActorService(IHost host)
        {
            if (host.LogService is null)
                throw new ArgumentNullException("Can't initialize IConfigService before ILogService");

            _host = host;
            _log = _host.LogService.GetContextLogger<IActorService>();
            var id = _host.Info.Id.ToString();
            MainActorSystem = ActorSystem.Create(id);
            _factories = new Dictionary<string, IActorFactory>();
            _instances = new Dictionary<string, IActorRef>();
        }

        public bool AddActorFactory(IActorFactory factory, bool replace = true)
        {
            if (!_factories.ContainsKey(factory.ActorRefName))
            {
                AddNewFactory(factory);
                return true;
            } 
            else if (replace)
            {
                ReplaceFactory(factory);
                return true;
            }

            return false;
        }

        public Option<IActorFactory> GetActorFactory(string name)
        {
            if (_factories.TryGetValue(name, out var factory))
                return Some(factory);
            return None<IActorFactory>();
        }

        public Option<IActorRef> GetActorOf(string name)
        {
            return GetActorFactory(name).Match(
                some: f => TryGetActorRef(f),
                none: () => NoActorFactory(name)
            );
        }

        public IEnumerable<IActorFactory> GetActorRefFactories() => _factories.Values;

        public bool HasActorFactory(string name) => _factories.ContainsKey(name);

        private void AddNewFactory(IActorFactory factory)
        {
            _log.Information("Adding actor factory for @{actorRefName}", factory.ActorRefName);
            _factories.Add(factory.ActorRefName, factory);
        }

        private void ReplaceFactory(IActorFactory factory)
        {
            _log.Information("Replacing actor factory for @{actorRefName}", factory.ActorRefName);

            var prev = _factories[factory.ActorRefName];
            if (prev.InstanceType == IActorRefInstanceType.Singleton)
                _log.Warning("Replacing non transient actor for @{actorRefName}", prev.ActorRefName);

            _factories[prev.ActorRefName] = factory;
        }

        private Option<IActorRef> TryGetActorRef(IActorFactory factory)
        {
            try
            {
                return GetActorRef(factory);
            }
            catch (Exception e)
            {
                _log.Error(e, "Error building actor for @{actorRefName}", factory.ActorRefName);
                return None<IActorRef>();
            }
        }

        private Option<IActorRef> GetActorRef(IActorFactory factory)
        {
            if (_instances.TryGetValue(factory.ActorRefName, out var actorRef))
            {
                _log.Information("Returning instance for @{actorRefName}", factory.ActorRefName);
                return Some(actorRef);
            }

            _log.Information("Creating actor for @{actorRefName}", factory.ActorRefName);
            var instance = factory.BuildActorRef(MainActorSystem);

            if (factory.InstanceType == IActorRefInstanceType.Singleton && instance.HasValue)
                _instances.Add(factory.ActorRefName, instance.ValueOrFailure());

            return instance;
        }

        private Option<IActorRef> NoActorFactory(string name)
        {
            _log.Information("Can't find actor factory for @{name}", name);
            return None<IActorRef>();
        }
    }
}
