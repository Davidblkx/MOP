using MOP.Core.Domain.RIP;
using MOP.Core.Domain.RIP.Factories;
using MOP.Core.Services;
using Serilog;
using System;
using System.Collections.Generic;

namespace MOP.Host.Services
{
    internal class RIPService : IRIPService
    {
        private readonly Dictionary<string, ICommand> _cmd;
        private readonly ILogger _log;

        public IEnumerable<ICommand> Commands => _cmd.Values;

        public RIPService(ILogService logService)
        {
            _cmd = new Dictionary<string, ICommand>();
            _log = logService.GetContextLogger<IRIPService>();
        }

        public Type? GetCommandType(string name)
            => GetCommand(name)?.Target;

        public bool HasName(string name)
            => _cmd.ContainsKey(name);

        public bool Register<T>(bool replace = false)
            => Register(typeof(T), replace);

        public ICommand? GetCommand(string name)
            => _cmd.ContainsKey(name) ? _cmd[name] : null;

        public bool Register(Type type, bool replace = false)
        {
            if (!(CommandFactory.ForType(type) is ICommand c))
                return false;

            if (HasName(c.Name))
                _log.Warning("Command for {@Name} already exists", c.Name);

            if (!HasName(c.Name) || replace)
            {
                _cmd[c.Name] = c;
                _log.Information("New command was registered: {@Name}", c);
            }

            return false;
        }
    }
}
