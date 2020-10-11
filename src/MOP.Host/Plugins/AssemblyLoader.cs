using MOP.Core.Infra.Tools;
using MOP.Core.Domain.Plugins;
using MOP.Core.Services;
using Optional;
using Serilog;
using System;
using System.IO;
using System.Reflection;

using static MOP.Core.Infra.Optional.Static;

namespace MOP.Host.Plugins
{
    internal class AssemblyLoader
    {
        private readonly ILogger _log;

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
                try
                {
                    if (!TypeTools.CanInstantiate<IPlugin>(t)) continue;
                    var instance = assembly.CreateInstance(t.FullName!) as IPlugin;
                    if (instance is null)
                        _log.Debug("Failed to instantiate type: {@FullName}", t.FullName);
                    else return Some(instance);
                }
                catch (Exception ex)
                {
                    _log.Error("Error instantiate IPlugin from {@FullName}", t.FullName);
                    _log.Debug(ex, "Error instantiate IPlugin");
                }
            }

            return None<IPlugin>();
        }
    }
}
