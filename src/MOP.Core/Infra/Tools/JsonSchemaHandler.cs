using NJsonSchema.Generation;
using System;

namespace MOP.Core.Infra.Tools
{
    public class JsonSchemaHandler
    {
        private readonly JsonSchemaGenerator _schemaGenerator;

        public JsonSchemaHandler()
        {
            var settings = new JsonSchemaGeneratorSettings { };
            _schemaGenerator = new JsonSchemaGenerator(settings);
        }

        public string Generate(Type type)
            => _schemaGenerator.Generate(type).ToJson();

        private static JsonSchemaHandler? handler;
        private static JsonSchemaHandler GetHandler()
        {
            if (handler is null)
                handler = new JsonSchemaHandler();
            return handler;
        }

        public static string GenerateSchema(Type type)
            => GetHandler().Generate(type);
    }
}
