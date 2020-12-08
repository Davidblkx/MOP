using MOP.Core.Services;
using System;
using System.IO;
using System.Reflection;
using Optional.Unsafe;

namespace MOP.Host.Plugins
{
    public class AssemblyResolver
    {
        private readonly IPluginService _pluginService;
        private readonly IInjectorService _injector;

        public AssemblyResolver(IPluginService pluginService, IInjectorService injector)
        {
            _pluginService = pluginService;
            _injector = injector;
        }

        public void RegisterAppDomain(AppDomain domain)
        {
            domain.AssemblyResolve += SearchAssembly;
        }

        private Assembly? SearchAssembly(object? sender, ResolveEventArgs args)
        {
            var domain = sender as AppDomain;
            if (FindAssembly(domain, args.Name) is Assembly a)
                return a;

            foreach (var i in _pluginService.PluginFolders)
                if (SearchInDirectory(i, args.Name) is Assembly ass)
                    return ass;
            
            return null;
        }

        private Assembly? FindAssembly(AppDomain? domain, string? fullName)
        {
            if (domain is null || fullName is null) return null;
            foreach (var a in domain.GetAssemblies())
                if (a.FullName == fullName)
                    return a;

            return null;
        }

        private Assembly? SearchInDirectory(DirectoryInfo info, string? name)
        {
            var loader = _injector.GetService<AssemblyLoader>();
            var files = info.GetFiles("*.dll", SearchOption.AllDirectories);
            foreach (var f in files)
            {
                var maybe = loader.LoadAssemblyFromFile(f);
                if (maybe.Exists(a => a.FullName == name))
                    return maybe.ValueOrFailure();
            }

            return null;
        }
    }
}