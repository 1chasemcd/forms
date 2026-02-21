using FormsApi;

namespace Sample;

public static class Program
{
    private static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddOpenApiDocument();

        builder.Services.AddForms(formsSetup => formsSetup
            .AddForm("home", new TestForm())
        );

        WebApplication app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseOpenApi();
            app.UseSwaggerUi();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.UseForms();

        app.Run();
    }
}