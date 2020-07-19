using MOP.Core.Domain.Plugins;
using MOP.Core.Services;
using Optional;
using Serilog;
using System;
using System.IO;
using System.Reflection;

using static MOP.Core.Optional.StaticOption;

namespace MOP.Host.Plugins
{
    internal class AssemblyLoader
    {
        private ILogger _log;

        public AssemblyLoader(ILogService logService)
        {
            _log = logService.GetContextLogger<AssemblyLoader>();
        }

        public Option<Assembly> LoadAssemblyFromFile(FileInfo file)
        {
            try
            {
                if (!file.Exists) throw new FileNotFoundException();

                var value = Assembly.LoadFrom(file.FullName);
                return Some(value);
            } 
            catch (FileNotFoundException ex)
            {
                _log.Error("Assembly can't be found in {@FullName}", file.FullName);
                _log.Debug(ex, "Assembly can't be found in {@FullName}", file.FullName);
            }
            catch (Exception ex)
            {
                _log.Error("Error loading assembly from: {@FullName}", file.FullName);
                _log.Debug(ex, "Error loading assembly from: {@FullName}", file.FullName);
            }

            return None<Assembly>();
        }

        public Option<IPlugin> LoadPluginFromAssembly(Assembly assembly)
        {
            foreach(var t in assembly.GetTypes())
            {
                if (t.FullName == "MOP.Core.Domain.Plugins.IPlugin") continue;
                var typed = t.GetInterface(nameof(IPlugin));
                if (t.IsAbstract || t.IsInterface || typed is null || t.FullName is null) continue;

                try
                {
                    var instance = assembly.CreateInstance(t.FullName) as IPlugin;
                    if (instance is null)
                        _log.Debug("Failed to instantiate type: {@FullName}", typed.FullName);
                    else return Some(instance);
                }
                catch (Exception ex)
                {
                    _log.Error("Error instantiate IPlugin from {@FullName}", typed.FullName);
                    _log.Debug(ex, "Error instantiate IPlugin");
                }
            }

            return None<IPlugin>();
        }
    }
}
