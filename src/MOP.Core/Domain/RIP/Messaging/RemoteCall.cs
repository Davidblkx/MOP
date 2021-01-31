using System;

namespace MOP.Core.Domain.RIP.Messaging
{
    public class RemoteCall : ICall
    {
        public RemoteCall() { }
        public RemoteCall(string cmd, string action)
        {
            Command = cmd;
            Action = action;
        }

        public string Command { get; set; } = "";

        public string Action { get; set; } = "";

        public string? Argument { get; set; }
    }
}
