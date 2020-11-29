using System;
using System.Collections.Generic;

namespace MOP.Core.Domain.RIP
{
    internal class Command : ICommand
    {
        public Command(string name, IEnumerable<IAction> actions, Type type)
        {
            Name = name;
            Actions = actions;
            Target = type;
        }

        public string Name { get; set; }

        public IEnumerable<IAction> Actions { get; set; }

        public string? Description { get; set; }

        public Type Target { get; set; }
    }
}
