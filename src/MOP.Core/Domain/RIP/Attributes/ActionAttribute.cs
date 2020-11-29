using System;

namespace MOP.Core.Domain.RIP.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ActionAttribute : Attribute
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ReturnDescription { get; set; }

        public ActionAttribute(string name) { Name = name; }
    }
}
