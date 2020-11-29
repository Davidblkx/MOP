using MOP.Core.Domain.RIP.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MOP.Core.Domain.RIP.Factories
{
    public class CommandFactory
    {
        private readonly Type _target;

        public bool IsValid => !(GetAttribute() is null);

        public CommandFactory(Type type)
            => _target = type;

        public ICommand Build()
        {
            var att = GetAttribute();
            if (att is null)
            {
                throw new ArgumentException("Type must have CommandAttribute");
            }

            var name = att.Name;
            var actions = LoadActions().ToList();
            var target = att.Target ?? _target;

            return new Command(name, actions, target)
            {
                Description = att.Description
            };
        }

        private CommandAttribute? GetAttribute()
            => _target.GetCustomAttribute<CommandAttribute>();

        private IEnumerable<MethodInfo> GetMethods()
            => _target.GetMethods(BindingFlags.Public | BindingFlags.Instance);

        private IEnumerable<IAction> LoadActions()
        {
            foreach (var m in GetMethods())
                if (ActionFactory.Build(m) is IAction a)
                    yield return a;
        }

        public static ICommand? ForType<T>()
            => ForType(typeof(T));

        public static ICommand? ForType(Type t)
        {
            var f = new CommandFactory(t);
            if (f.IsValid) return f.Build();
            return null;
        }
    }
}
