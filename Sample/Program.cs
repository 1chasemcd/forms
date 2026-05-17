using FormsApi;
using FormsApi.Contract;
using NJsonSchema;
using NJsonSchema.Generation.TypeMappers;
using Sample.Grid;
using Sample.Home;

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
            config.SchemaSettings.TypeMappers.Add(new PrimitiveTypeMapper(typeof(TypeDto), schema =>
            {
                schema.Type = JsonObjectType.String;
                schema.Description = "Base64-encoded assembly-qualified type name";
            }));
        });


        builder.Services.AddForms(formsSetup => formsSetup
            .AddForm<HomeForm>("home")
            .AddForm<GridForm>("grid")
            .AddForm<UserEditForm>("user")
            .AddRepository<HomeService>()
            .AddRepository<UserRepository>()
        );

        builder.Services.AddSingleton<HomeService>();

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
