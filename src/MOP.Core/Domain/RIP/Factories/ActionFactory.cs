using MOP.Core.Domain.RIP.Attributes;
using System;
using System.Linq;
using System.Reflection;

using static MOP.Core.Infra.Tools.JsonSchemaHandler;
using static MOP.Core.Infra.Tools.TypeTools;

namespace MOP.Core.Domain.RIP.Factories
{
    internal class ActionFactory
    {
        private readonly MethodInfo _methodInfo;

        public bool IsValid => !(GetAttribute() is null);

        public ActionFactory(MethodInfo info) { _methodInfo = info; }

        public IAction Build()
        {
            var att = GetAttribute();
            if (att is null)
            {
                throw new ArgumentException("Method does not implement the ActionAttribute");
            }

            var arg = _methodInfo.GetParameters().FirstOrDefault();
            var argType = arg?.ParameterType.AssemblyQualifiedName ?? GetVoidName();
            var returnType = _methodInfo.ReturnType.AssemblyQualifiedName;
            var argSchema = arg is null ? "" : GenerateSchema(arg.ParameterType);
            var returnSchema = GenerateSchema(_methodInfo.ReturnType);

            return new Action(att.Name)
            {
                ArgumentSchema = argSchema,
                ArgumentType = argType,
                Description = att.Description,
                ReturnDescription = att.ReturnDescription,
                ReturnSchema = returnSchema,
                ReturnType = returnType
            };
        }

        private ActionAttribute? GetAttribute()
            => _methodInfo.GetCustomAttribute<ActionAttribute>();

        public static IAction? Build(MethodInfo info)
        {
            var f = new ActionFactory(info);
            if (f.IsValid) return f.Build();
            return null;
        }
    }
}
