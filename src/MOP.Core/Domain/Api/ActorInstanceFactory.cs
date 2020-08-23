using MOP.Core.Infra.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MOP.Core.Domain.Api
{
    public class ActorInstanceFactory
    {
        private readonly Type _target;

        public ActorInstanceFactory(Type target)
        {
            _target = target;
        }

        public bool IsValid()
        {
            return !(GetGroupAttribute() is null);
        }

        public ActorInstance Build()
        {
            var groupAttr = GetGroupAttribute();
            if (groupAttr is null)
                throw new ArgumentNullException($"Type {_target} does not have a CommandApiGroupAttribute");
            var cmd = new ActorInstance { Path = groupAttr.Path };
            if (!groupAttr.Description.IsNullOrEmpty())
                cmd.Description = groupAttr.Description;

            List<ActorAction> actions = new List<ActorAction>();
            var publicMethods = _target.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            var privateMethods = _target.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
            var allMethods = publicMethods.Concat(privateMethods);

            foreach (var m in allMethods)
            {
                var factory = new ActorActionFactory(m);
                if (!factory.IsValid()) continue;
                actions.Add(factory.Build());
            }

            cmd.Commands = actions;
            return cmd;
        }

        private ActorInstanceAttribute? GetGroupAttribute()
        {
            return _target.GetCustomAttributes(true)
                .FirstOrDefault(e => e is ActorInstanceAttribute)
                as ActorInstanceAttribute;
        }

        public static ActorInstanceFactory ForType<T>()
            => new ActorInstanceFactory(typeof(T));
    }
}
