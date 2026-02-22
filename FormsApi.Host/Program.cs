using FormsApi.Form.Primitives;
using NJsonSchema;
using NJsonSchema.Generation;
using NJsonSchema.Generation.TypeMappers;

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
        if (context.ContextualType == typeof(RepositoryType))
        {
            context.Schema.Type = JsonObjectType.String;
            context.Schema.Properties.Clear();

        }
    }
}
