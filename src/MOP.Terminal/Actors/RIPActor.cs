using Akka.Actor;
using MOP.Core.Domain.RIP.Messaging;
using MOP.Terminal.Services;
using MOP.Terminal.Models;
using Serilog;
using MOP.Terminal.Factories;
using MOP.Core.Domain.RIP;
using System;

namespace MOP.Terminal.Actors
{
    internal class RIPActor : ReceiveActor
    {
        private readonly ILogger _log;
        private readonly ActorSelection _hostActor;

        public RIPActor(ILogService logService, IHostConfig host)
        {
            _log = logService.ForContext<RIPActor>();
            _hostActor = Context.ActorSelection(AkkaAddressFactory.BuildTcp(host, "RIP"));
            ConfigureReceivers();
        }

        private void ConfigureReceivers()
        {
            Receive<Response>(OnResponse);
            Receive<ICall>(OnRequest);
        }

        public void OnRequest(ICall call)
        {
            _hostActor.Tell(call);
        }

        public void OnResponse(Response res)
            => HandleResponse(res);

        private int HandleResponse(Response res)
        {
            if (!res.Valid)
            {
                _log.Warning("[@{Code}] Error: {@Message}", res.Error?.code, res.Error?.message);
                return 1;
            }
            else if (res.Body is string str)
            {
                Console.WriteLine(str);
                return 0;
            }

            _log.Warning("Can't read response");
            return 2;
        }
    }
}
