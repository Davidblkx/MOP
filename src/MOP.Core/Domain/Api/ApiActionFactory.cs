using System;
using System.Linq;
using System.Reflection;

namespace MOP.Core.Domain.Api
{
    public class ApiActionFactory
    {
        private readonly MethodInfo _target;

        public bool IsValid => !(GetAttribute() is null);

        public ApiActionFactory(MethodInfo target)
        {
            _target = target;
        }

        public ApiAction Build()
        {
            var att = GetAttribute();
            if (att is null) { 
                throw new ArgumentException("Method does not implement the ApiActionAttribute"); 
            }

            var arg = _target.GetParameters().FirstOrDefault();
            var messageType = arg?.ParameterType.AssemblyQualifiedName ?? GetVoidName();
            var returnType = _target.ReturnType.AssemblyQualifiedName;
            var messageSchema = arg is null ? "" : ApiSchemaHandler.GenerateSchema(arg.ParameterType);
            var returnSchema = ApiSchemaHandler.GenerateSchema(_target.ReturnType);

            return new ApiAction(_target.Name)
            {
                Description = att.Description ?? "No description",
                ReturnDescription = att.Returns ?? "No description",
                MessageType = messageType,
                ReturnType = returnType,
                MessageTypeSchema = messageSchema,
                ReturnTypeSchema = returnSchema
            };
        }

        private string GetVoidName()
            => typeof(void).AssemblyQualifiedName;

        private ApiActionAttribute? GetAttribute()
            => _target.GetCustomAttribute<ApiActionAttribute>();
    }
}
