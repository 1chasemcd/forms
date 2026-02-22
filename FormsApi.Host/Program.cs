using FormsApi.Form;

namespace FormsApi.Host;

public static class Program
{
    private static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers()
            .AddApplicationPart(typeof(FormController).Assembly);

        builder.Services.AddOpenApiDocument();
        WebApplication app = builder.Build();
        app.UseOpenApi();
    }
}
