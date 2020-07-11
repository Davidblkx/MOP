using MOP.Core.Domain.Host;
using MOP.Core.Services;
using Newtonsoft.Json.Linq;
using Optional;
using System;
using System.IO;
using System.Threading.Tasks;
using MOP.Host.Helpers;
using Serilog;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Serilog.Events;

using static MOP.Core.Optional.StaticOption;

[assembly: InternalsVisibleTo("MOP.Host.Test")]
namespace MOP.Host.Services
{
    internal class ConfigService : IConfigService
    {
        private const string FILE_NAME = ".config.json";
        private readonly ILogger _log;
        private readonly IHost _host;
        private readonly JsonLoadSettings _jsonLoadSettings = new JsonLoadSettings
        {
            CommentHandling = CommentHandling.Ignore,
            DuplicatePropertyNameHandling = DuplicatePropertyNameHandling.Replace,
            LineInfoHandling = LineInfoHandling.Load
        };
        private JObject? _configObj;
        private bool _hasInit = false;

        public ConfigService(IHost host)
        {
            if (host.LogService is null)
                throw new ArgumentNullException("Can't initialize IConfigService before ILogService");

            _host = host;
            _log = host.LogService.GetContextLogger<ConfigService>();
        }

        public async Task<bool> ReloadConfigObject(bool force = false)
        {
            if (!force && _hasInit) return true;
            _log.Information("Starting loading config file");

            try
            {
                _configObj = LoadConfigObject(await ReadConfigFile(GetFileInfo()));
                _log.Information("Loading config file completed");
                return true;
            }
            catch (Exception e)
            {
                _log.Error("Error loading config file", e);
                return false;
            }
            finally
            {
                _hasInit = true;
            }
        }

        public async Task<Option<T>> LoadConfig<T>(Guid id, T defaultValue = default)
        {
            await ReloadConfigObject();
            _log.Information("Loading config for: {@id}", id);
            if (_configObj is null) return LogAndReturnInitFail(None<T>());

            try
            {
                var tokenValue = _configObj.GetValue(id.ToString());
                if (tokenValue is null) return LogAndReturnMissingId(id, defaultValue);

                if (tokenValue.ToObject<T>() is T value)
                    return Some(value);
                return LogAndReturnCastErrorId(id, defaultValue);
            }
            catch (ArgumentException e)
            {
                _log.Error(e, "Error loading config object to target, id: {@id}", id);
                return LogAndReturnMissingId(id, defaultValue);
            }
            catch (Exception e)
            {
                _log.Error(e, "Error casting config object to target, id: {@id}", id);
                return LogAndReturnCastErrorId(id, defaultValue);
            }
        }

        public async Task<bool> SaveConfig<T>(Guid id, T value)
        {
            _log.Information("Saving config for: {@id}", id);
            if (_configObj is null) return LogAndReturnInitFail(false);
            if (value is null) return LogAndReturnSaveNull(id);

            try
            {
                var jValue = JObject.FromObject(value);

                if (_configObj.ContainsKey(id.ToString()))
                    _configObj[id] = jValue;
                else 
                    _configObj.Add(id.ToString(), jValue);

                return await SaveConfig();
            }
            catch (Exception e)
            {
                _log.Error(e, "Error saving config for id: {@id}", id);
                return false;
            }
        }

        private Option<T> LogAndReturnCastErrorId<T>(Guid id, T value)
            => LogAndReturn(Some(value), LogEventLevel.Warning, "Can't cast {@id} to target", id);

        private Option<T> LogAndReturnMissingId<T>(Guid id, T value)
            => LogAndReturn(Some(value), LogEventLevel.Warning, "Can't find config for {@id}", id);

        private T LogAndReturnInitFail<T>(T value)
            => LogAndReturn(value, LogEventLevel.Warning, "Initialization of config failed");

        private bool LogAndReturnSaveNull(Guid id)
            => LogAndReturn(false, LogEventLevel.Warning, "Can't save a null config for {@id}", id);

        private T LogAndReturn<T>(T value, LogEventLevel level, string message, params object[] propValues)
        {
            _log.Write(level, message, propValues);
            return value;
        }

        private async Task<string> ReadConfigFile(FileInfo configFile)
        {
            if (!configFile.Exists)
                return "{}";

            return await File.ReadAllTextAsync(configFile.FullName);
        }

        private JObject LoadConfigObject(string jsonBody)
            => JObject.Parse(jsonBody, _jsonLoadSettings);

        private FileInfo GetFileInfo()
            => _host.DataDirectory.RelativeFile(FILE_NAME);

        private Task SaveConfigFile(FileInfo configFile, string body)
            => File.WriteAllTextAsync(configFile.FullName, body);

        private string SerializeConfig(JObject obj)
            => JsonConvert.SerializeObject(obj, Formatting.Indented);

        private async Task<bool> SaveConfig()
        {
            try
            {
                if (_configObj is null)
                    throw new ArgumentNullException("Trying to save before loading the config file");

                _log.Information("Starting saving config file");
                await SaveConfigFile(GetFileInfo(), SerializeConfig(_configObj));
                _log.Information("Saving config file completed");
                return true;
            }
            catch (Exception e)
            {
                _log.Error("Error loading config file", e);
                return false;
            }
        }
    }

    internal static class ConfigServiceBuilder
    {
        public static async Task<IConfigService> Build(IHost host)
        {
            var service = new ConfigService(host);
            await service.ReloadConfigObject(true);
            return service;
        }
    }
}
