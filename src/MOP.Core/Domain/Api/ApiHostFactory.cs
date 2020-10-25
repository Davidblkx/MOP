using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MOP.Core.Domain.Api
{
    public class ApiHostFactory
    {
        private readonly Type _target;

        public bool IsValid => !(GetAttribute() is null);

        public ApiHostFactory(Type target)
        {
            _target = target;
        }

        public ApiHost Build()
        {
            var att = GetAttribute();
            if (att is null)
            {
                throw new ArgumentException("Type must have ApiHostAttribute");
            }

            return new ApiHost(att.Path, att.Name)
            {
                Description = att.Description ?? "No description",
                Actions = GetActions(),
            };
        }

        private ApiHostAttribute? GetAttribute()
            => _target.GetCustomAttribute<ApiHostAttribute>();

        private IEnumerable<MethodInfo> GetMethods()
        {
            var publicMethods = _target.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            var privateMethods = _target.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
            return publicMethods.Concat(privateMethods);
        }

        private IEnumerable<ApiAction> GetActions()
        {
            foreach(var m in GetMethods())
            {
                var f = new ApiActionFactory(m);
                if (f.IsValid) yield return f.Build();
            }
        }

        public static ApiHost ForType<T>()
            => new ApiHostFactory(typeof(T)).Build();
    }
}
