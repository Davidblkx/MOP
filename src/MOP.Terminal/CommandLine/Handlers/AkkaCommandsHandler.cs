using MOP.Terminal.CommandLine.Commands;
using Newtonsoft.Json;
using System;
using System.CommandLine.Facilitator;
using System.Threading.Tasks;
using MOP.Core.Infra.Extensions;
using MOP.Terminal.Infra;
using MOP.Terminal.Services;

namespace MOP.Terminal.CommandLine.Handlers
{
    [HandlerHost]
    internal class AkkaCommandsHandler
    {
        private readonly ISettingsService _settings;

        public AkkaCommandsHandler(ISettingsService s)
        {
            _settings = s;
        }

        [Handler(Target = typeof(AkkaCommandList))]
        public int List(AkkaCommandList o)
        {
            var format = o.Format ? Formatting.Indented : Formatting.None;
            var txt = JsonConvert.SerializeObject(_settings, format);
            Console.WriteLine(txt);
            return 0;
        }

        [Handler(Target = typeof(AkkaCommandSet))]
        public async Task<int> Set(AkkaCommandSet o)
        {
            if (o.Key.IsNullOrEmpty())
            {
                Console.WriteLine("Key is invalid".Error());
                return 1;
            }

            var type = _settings.ActorSystem.GetType();
            var prop = type.GetProperty(o.Key);

            if (prop is null)
            {
                Console.WriteLine($"Can't find key: {o.Key}".Error());
                return 1;
            }

            object? toSet = o.Value;
            bool isValid = true;
            try { toSet = Convert.ChangeType(o.Value, prop.PropertyType); }
            catch { isValid = false; }

            if (!isValid || toSet is null)
            {
                Console.WriteLine($"Invalid value: {o.Value}".Error());
                return 1;
            }

            prop.SetValue(_settings.ActorSystem, toSet);
            await _settings.SaveAsync();

            return 0;
        }
    }
}