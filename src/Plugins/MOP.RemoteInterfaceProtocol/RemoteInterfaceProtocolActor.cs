using Newtonsoft.Json;
using System.Reflection;
using MOP.Core.Domain.RIP.Messaging;
using Akka.Actor;
using MOP.Core.Domain.RIP;
using MOP.Core.Services;
using Serilog;
using System;
using System.Linq;
using MOP.Core.Infra.Extensions;

using static MOP.RemoteInterfaceProtocol.ErrorMap;
using static MOP.Core.Infra.Tools.TypeTools;

namespace MOP.RemoteInterfaceProtocol
{
    public class RemoteInterfaceProtocolActor : ReceiveActor
    {
        private readonly ILogger _log;
        private readonly IRIPService _rip;
        private readonly IInjectorService _injector;

        public RemoteInterfaceProtocolActor(ILogService logService, IRIPService rip, IInjectorService injector)
        {
            _log = logService.GetContextLogger<RemoteInterfaceProtocolActor>();
            _rip = rip;
            _injector = injector;
            RegisterReceivers();
        }

        private void RegisterReceivers()
        {
            Receive<ICall>(c => OnCall(c));
            Receive<string>(m => OnString(m));
        }

        private void OnString(string message)
        {
            _log.Debug(message);
        }

        private void OnCall(ICall call)
        {
            try
            {
                _log.Debug("Invoke request for {@Action} in {@Command}", call.Action, call.Command);
                Invoke(call);
            } catch (Exception ex) {
                Sender.Tell(BuildError(RIPErrors.InternalError, "On call receive", ex));
            }
        }

        private void Invoke(ICall call)
        {
            if (!(_rip.GetCommand(call.Command) is ICommand c))
            {
                Sender.Tell(BuildError(RIPErrors.CommandNotFound, call.Command));
                return;
            }

            if (!(c.Actions.FirstOrDefault(e => e.Name.EqualIgnoreCase(call.Action)) is IAction a))
            {
                Sender.Tell(BuildError(RIPErrors.ActionNotFound, call.Action));
                return;
            }

            var response = Execute(call, c, a);

            if (a.ReturnType.Equals(GetVoidName()))
                Sender.Tell(Response.Success(), Self);
            else
                Sender.Tell(Response.Success(response), Self);
        }

        private object? Execute(ICall call, ICommand c, IAction a)
        {
            var service = _injector.GetService(c.Target);
            if (service is null)
                throw new NullReferenceException("Can't find service for type " + c.Target.FullName);

            var method = c.Target.GetMethod(a.Name);
            if (method is null)
                throw new NullReferenceException("Can't find service for type " + c.Target.FullName);

            var args = DeserializeArgument(method, call.Argument);

            return method.Invoke(service, args);
        }

        private object?[] DeserializeArgument(MethodInfo info, string? argument)
        {
            if (info.GetParameters().Length == 0)
                return new object?[0];

            if (argument is null)
                return new object?[] { null };

            var res = JsonConvert.DeserializeObject(argument, info.GetParameters().First().ParameterType);
            return new object?[] { res };
        }
    }
}
