using FormsApi;
using FormsApi.Form.Primitives;
using NJsonSchema;
using NJsonSchema.Generation.TypeMappers;

namespace Sample;

public static class Program
{
    private static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services
            .AddControllers()
            .AddFormControllers();

        builder.Services.AddOpenApiDocument(config =>
        {
            config.SchemaSettings.TypeMappers.Add(new PrimitiveTypeMapper(typeof(RepositoryType), schema =>
            {
                schema.Type = JsonObjectType.String;
                schema.Description = "Base64-encoded assembly-qualified type name";
            }));
        });


        builder.Services.AddForms(formsSetup => formsSetup
            .AddForm<TestForm>("home")
            .AddRepository<ModelRepository>()
        );

        WebApplication app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseOpenApi();
            app.UseSwaggerUi();
        }
        if (app.Environment.IsProduction())
            app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.UseForms();

        app.Run();
    }
}
