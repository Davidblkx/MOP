using System;

namespace MOP.Core.Domain.Api
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ActorActionAttribute : Attribute
    {
        public string? Description { get; set; }
        public ActorActionAttribute(string? description = default)
        {
            Description = description;
        }
    }
}
