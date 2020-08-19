using System;
using System.Collections.Generic;
using System.Text;

namespace MOP.Terminal.ActorsSystem
{

    public class ServerMessage
    {
        public object Message { get; set; }

        public ServerMessage(object message)
        { Message = message; }

        public static ServerMessage Create(object message)
            => new ServerMessage(message);
    }
}
