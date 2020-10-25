using NJsonSchema.Generation;
using System;

namespace MOP.Core.Domain.Api
{
    /// <summary>
    /// Generates schema from type
    /// </summary>
    public class ApiSchemaHandler
    {
        private readonly JsonSchemaGenerator _schemaGenerator;

        public ApiSchemaHandler()
        {
            var settings = new JsonSchemaGeneratorSettings { };
            _schemaGenerator = new JsonSchemaGenerator(settings);
        }

        public string Generate(Type type)
            => _schemaGenerator.Generate(type).ToJson();

        private static ApiSchemaHandler? handler;
        private static ApiSchemaHandler GetHandler()
        {
            if (handler is null)
                handler = new ApiSchemaHandler();
            return handler;
        }

        public static string GenerateSchema(Type type)
            => GetHandler().Generate(type);
    }
}
