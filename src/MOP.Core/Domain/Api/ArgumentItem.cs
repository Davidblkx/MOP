using System;
using System.Reflection;

namespace MOP.Core.Domain.Api
{
    public class ArgumentItem
    {
        public Type ArgumentType { get; set; }
        public string Name { get; set; }
        public object? DefaultValue { get; set; }
        public bool IsOptional { get; set; }
        public bool HasDefaultValue { get; set; }
        public int Position { get; set; }

        public ArgumentItem()
        {
            ArgumentType = typeof(object);
            Name = "UNKOWN";
            IsOptional = false;
            HasDefaultValue = false;
        }

        public ArgumentItem(ParameterInfo info)
        {
            ArgumentType = info.ParameterType;
            DefaultValue = info.DefaultValue;
            Name = info.Name;
            IsOptional = info.IsOptional;
            HasDefaultValue = info.HasDefaultValue;
            Position = info.Position;
        }
    }
}
