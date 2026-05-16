using FormsApi.Definition.Primitives;
using NJsonSchema;
using NJsonSchema.Generation;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace FormsApi.Host;

public static class Program
{
    private static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers().AddFormControllers();

        builder.Services.AddOpenApiDocument(config =>
        {
            config.SchemaSettings.SchemaProcessors.Add(new RepositoryTypeSchemaProcessor());
            config.DocumentProcessors.Add(new IncludeTypesDocumentProcessor());
        });
        WebApplication app = builder.Build();
        app.UseOpenApi();
    }
}

public class RepositoryTypeSchemaProcessor : ISchemaProcessor
{
    public void Process(SchemaProcessorContext context)
    {
        if (context.ContextualType == typeof(TypeDto))
        {
            context.Schema.Type = JsonObjectType.String;
            context.Schema.Properties.Clear();

        }
    }
}

public class IncludeTypesDocumentProcessor : IDocumentProcessor
{
    public void Process(DocumentProcessorContext context)
    {
        JsonSchema schema = context.SchemaGenerator.Generate(
            typeof(RecalculateEventDto),
            context.SchemaResolver
        );

        context.Document.Components.Schemas["RecalculateEvent"] = schema;
    }
}
