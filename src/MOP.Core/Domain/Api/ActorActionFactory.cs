using MOP.Core.Infra.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace MOP.Core.Domain.Api
{
    public class ActorActionFactory
    {
        private readonly MethodInfo _target;

        public ActorActionFactory(MethodInfo target)
        {
            _target = target;
        }

        public bool IsValid()
            => !(GetAttribute() is null);

        public ActorAction Build()
        {
            var attr = GetAttribute();
            if (attr is null)
                throw new ArgumentNullException($"Method {_target.Name} does not have a CommandApiGroupAttribute");

            var cmd = new ActorAction()
            {
                Name = _target.Name,
                ReturnType = _target.ReturnType
            };
            if (!attr.Description.IsNullOrEmpty())
                cmd.Description = attr.Description;

            var args = new Dictionary<int, ArgumentItem>();
            foreach (var i in _target.GetParameters())
            {
                args.Add(i.Position, new ArgumentItem(i));
            }

            cmd.Arguments = args;
            return cmd;
        }

        private ActorActionAttribute? GetAttribute()
        {
            return _target.GetCustomAttribute<ActorActionAttribute>();
        }
    }
}
