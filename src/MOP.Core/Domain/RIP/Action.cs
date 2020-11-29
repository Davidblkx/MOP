using static MOP.Core.Infra.Tools.TypeTools;

namespace MOP.Core.Domain.RIP
{
    internal class Action : IAction
    { 
        public Action(string name) {
            Name = name;
            ArgumentType = GetVoidName();
            ReturnType = GetVoidName();
        }

        public string Name { get; set; }

        public string? Description { get; set; }

        public string? ReturnDescription { get; set; }

        public string ArgumentType { get; set; }

        public string ArgumentSchema { get; set; } = "{}";

        public string ReturnType { get; set; }

        public string ReturnSchema { get; set; } = "{}";
    }
}
