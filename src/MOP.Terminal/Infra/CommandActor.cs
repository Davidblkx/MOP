using System;

namespace MOP.Terminal.Infra
{
    [AttributeUsage(AttributeTargets.Class)]
    internal class CommandActor : Attribute
    {
        public string Name { get; set; }

        public CommandActor(string name)
        { Name = name; }
    }
}
