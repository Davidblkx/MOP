using System.Collections.Generic;

namespace MOP.Core.Domain.Api
{
    public class ActorInstance
    {
        public string Path { get; set; }
        public IEnumerable<ActorAction> Commands { get; set; }
        public string Description { get; set; }

        public ActorInstance()
        {
            Path = "default";
            Commands = new List<ActorAction>();
            Description = "Group action for " + Path;
        }
    }
}
