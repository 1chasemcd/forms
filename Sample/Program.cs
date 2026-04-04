using FormsApi;
using FormsApi.Definition.Primitives;
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
            config.SchemaSettings.TypeMappers.Add(new PrimitiveTypeMapper(typeof(SerializedType), schema =>
            {
                schema.Type = JsonObjectType.String;
                schema.Description = "Base64-encoded assembly-qualified type name";
            }));
        });


        builder.Services.AddForms(formsSetup => formsSetup
            .AddForm<TestForm>("home")
            .AddForm<GridForm>("grid")
            .AddForm<UserEditForm>("user")
            .AddRepository<ModelRepository>()
            .AddRepository<UserRepository>()
        );

        builder.Services.AddSingleton<TestService>();

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
