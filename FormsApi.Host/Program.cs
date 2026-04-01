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

        var schema = JsonSchema.FromType<FormElementSize>();

        builder.Services.AddOpenApiDocument(config =>
        {
            config.SchemaSettings.SchemaProcessors.Add(new RepositoryTypeSchemaProcessor());
            config.SchemaSettings.SchemaProcessors.Add(new FormElementSizeSchemaProcessor());
            config.DocumentProcessors.Add(new IncludeTypesDocumentProcessor());
        });
        WebApplication app = builder.Build();
        app.UseOpenApi();
    }
}


public class FormElementSizeSchemaProcessor : ISchemaProcessor
{
    public void Process(SchemaProcessorContext context)
    {
        if (context.ContextualType == typeof(FormElementSize))
        {
            context.Schema.Type = JsonObjectType.Integer;
            context.Schema.Properties.Clear();
        }
    }
}

public class RepositoryTypeSchemaProcessor : ISchemaProcessor
{
    public void Process(SchemaProcessorContext context)
    {
        if (context.ContextualType == typeof(SerializedType))
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
            typeof(RecalculateEvent),
            context.SchemaResolver
        );

        context.Document.Components.Schemas["RecalculateEvent"] = schema;
    }
}
