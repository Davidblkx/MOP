using System;
using System.CommandLine.Facilitator;
using System.Threading.Tasks;
using MOP.Terminal.CommandLine.Commands;
using MOP.Terminal.Infra;
using MOP.Terminal.Services;
using Newtonsoft.Json;

namespace MOP.Terminal.CommandLine.Handlers
{
    [HandlerHost]
    class SettingsCommandsHandler
    {
        private readonly ISettingsService _settings;

        public SettingsCommandsHandler(ISettingsService s)
        {
            _settings = s;
        }

        [Handler(Target = typeof(SettingsCommandPrint))]
        public int Print(SettingsCommandPrint o)
        {
            var format = o.Format ? Formatting.Indented : Formatting.None;
            object obj = o.All ? _settings : new
            {
                _settings.DefaultHost,
                _settings.Id,
                _settings.LogLevel,
                _settings.LogToFile
            };
            var txt = JsonConvert.SerializeObject(obj, format);
            Console.WriteLine(txt);
            return 0;
        }

        [Handler(Target = typeof(SettingsCommandSet))]
        public async Task<int> Change(SettingsCommandSet o)
        {
            _settings.DefaultHost = o.DefaultHost ?? _settings.DefaultHost;
            _settings.Id = o.Id ?? _settings.Id;
            _settings.LogLevel = o.LogLevel ?? _settings.LogLevel;
            _settings.LogToFile = o.LogToFile ?? _settings.LogToFile;

            if (o.ClearDefaultHost) _settings.DefaultHost = null;
            await _settings.SaveAsync();
            Console.WriteLine("Settings updated".Success());
            return 0;
        }
    }
}
