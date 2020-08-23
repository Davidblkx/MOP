using System;
using System.Collections.Generic;

namespace MOP.Core.Domain.Api
{
    /// <summary>
    /// Represents an API command that can be called by third party applications
    /// </summary>
    public class ActorAction
    {
        public string Name { get; set; }
        public Dictionary<int, ArgumentItem> Arguments { get; set; }
        public Type ReturnType { get; set; }
        public string Description { get; set; }

        public ActorAction()
        {
            Name = "DEFAULT";
            Arguments = new Dictionary<int, ArgumentItem>();
            ReturnType = typeof(void);
            Description = "Invokes action " + Name;
        }
    }
}
