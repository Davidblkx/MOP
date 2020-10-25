using MOP.Core.Domain.Api;
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
        private readonly List<ApiHost> _apiCollection;
        private readonly ILogger _logger;

        public ApiService(ILogService logService)
        {
            _apiCollection = new List<ApiHost>();
            _logger = logService.GetContextLogger<ApiService>();
        }

        public void Add(ApiHost api)
        {
            if (_apiCollection.Count(e => e.Path == api.Path) > 0)
            {
                _logger.Warning($"API already contains a description for path: {api.Path}");
            }

            _apiCollection.Add(api);
        }

        public void Add(Type type)
        {
            var factory = new ApiHostFactory(type);
            if (factory.IsValid) Add(factory.Build());
        }

        public void Add(Assembly assembly)
        {
            _logger.Debug($"Loading API for {assembly.FullName}");
            foreach (var t in assembly.GetTypes()) { Add(t); }
        }

        public IEnumerable<ApiHost> GetAll() => _apiCollection;

        public Option<ApiHost> GetByPathOrName(string nameOrPath)
            => Some(_apiCollection.FirstOrDefault(
                e => e.Path == nameOrPath 
                || e.Name == nameOrPath
            ));
    }
}
