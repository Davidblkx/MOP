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
using Newtonsoft.Json.Linq;

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
            if (_rip.GetCommand(call.Command) is not ICommand c)
            {
                Sender.Tell(BuildError(RIPErrors.CommandNotFound, call.Command));
                return;
            }

            if (c.Actions.FirstOrDefault(e => e.Name.EqualIgnoreCase(call.Action)) is not IAction a)
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

        private static object?[] DeserializeArgument(MethodInfo info, string? argument)
        {
            if (info.GetParameters().Length == 0)
                return Array.Empty<object?>();

            if (argument is null)
                return GetDefaultValuesForParameters(info);

            return DeserializeArgument(info.GetParameters(), argument);
        }

        private static object?[] GetDefaultValuesForParameters(MethodInfo info)
        {
            var pList = info.GetParameters()
                    .Select(p => (p.Position, p.ParameterType));
            var args = new object?[pList.Count()];
            foreach (var (Position, ParameterType) in pList)
            {
                var value = DefaultValue(ParameterType);
                args[Position] = value;
            }
            return args;
        }

        private static object?[] DeserializeArgument(ParameterInfo[] parameters, string args)
        {
            var res = new object?[parameters.Length];
            var arr = JArray.Parse(args);

            for (var i = 0; i < res.Length; i++)
            {
                var p = parameters.First(e => e.Position == i);

                res[i] = i < arr.Count ?
                    arr[i].ToObject(p.ParameterType)
                    : DefaultValue(p.ParameterType);
            }

            return res;
        }

        private static object? DefaultValue(Type t)
            => t.IsValueType ? Activator.CreateInstance(t) : null;
    }
}
