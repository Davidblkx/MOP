using MOP.Core.Domain.Api;
using MOP.Core.Services;
using MOP.Core.Infra.Extensions;
using MOP.Core.Services;
using Optional;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using static MOP.Core.Infra.Optional.Static;

namespace MOP.Host.Services
{
    public class ApiService : IApiService
    {
        private readonly List<ActorInstance> _actorsInstancesApi;
        private readonly ILogger _logger;

        public ApiService(ILogService logService)
        {
            _actorsInstancesApi = new List<ActorInstance>();
            _logger = logService.GetContextLogger<ApiService>();
        }

        public void Add(ActorInstance instance)
            => _actorsInstancesApi.Add(instance);

        public void Add(Type type)
        {
            var factory = new ActorInstanceFactory(type);
            if (!factory.IsValid())
            {
                _logger.Debug("Type {FullName} doesn't support API", type.FullName);
                return;
            }
            Add(factory.Build());
        }

        public void Add(Assembly assembly)
        {
            _logger.Debug("Loading assembly {@FullName}", assembly.FullName);
            foreach (var t in assembly.GetTypes())
            {
                var factory = new ActorInstanceFactory(t);
                if (factory.IsValid())
                    Add(factory.Build());
            }
        }

        public IEnumerable<ActorInstance> GetAll()
            => _actorsInstancesApi;

        public Option<ActorInstance> GetByPath(string path)
            => Some(_actorsInstancesApi.FirstOrDefault(
                    e => e.Path.EqualIgnoreCase(path)
                ));
    }
}
