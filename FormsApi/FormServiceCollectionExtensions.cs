using System.Text.Json;
using System.Text.Json.Serialization;
using FormsApi.Forms.Services;
using FormsApi.Metadata.Services;
using FormsApi.Repository.Handlers;
using FormsApi.Repository.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FormsApi;

public static class FormServiceCollectionExtensions
{
    public static IMvcBuilder AddFormControllers(this IMvcBuilder builder)
    {
        builder.AddApplicationPart(typeof(FormServiceCollectionExtensions).Assembly);
        builder.AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.DefaultIgnoreCondition =
                System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        });

        return builder;
    }

    public static IServiceCollection AddForms(this IServiceCollection services, Action<IFormSetupOptions>? setupAction)
    {
        services.TryAddSingleton<IFormRegistry, FormRegistry>();
        services.TryAddSingleton<IMetadataBuilderService, MetadataBuilderService>();
        services.TryAddSingleton<MetadataProcessors>();
        services.TryAddSingleton<IRepositoryServiceFactory, RepositoryServiceFactory>();
        services.TryAddSingleton<IRepositoryResolver, RepositoryResolver>();
        services.TryAddSingleton(typeof(DefaultRepositoryCreateHandler<>));

        if (setupAction != null)
        {
            var options = new FormSetupOptions(services);
            setupAction.Invoke(options);
            services.AddSingleton(options);
        }

        return services;
    }
}
