using FormsApi.Builder.Validation;
using FormsApi.Common.Registry;
using FormsApi.Form.Json;
using FormsApi.Repository;
using FormsApi.Repository.Service;
using FormsApi.Setup;
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
            options.JsonSerializerOptions.PropertyNamingPolicy = null;
        });

        return builder;
    }

    public static IServiceCollection AddForms(this IServiceCollection services, Action<IFormSetupOptions>? setupAction)
    {
        services.TryAddSingleton<FormRegistry>();
        services.TryAddSingleton<IRepositoryServiceFactory, RepositoryServiceFactory>();
        services.TryAddSingleton<IRepositoryResolver, RepositoryResolver>();
        services.TryAddSingleton(typeof(DefaultRepository<>));
        services.TryAddSingleton<FormSetupService>();
        services.TryAddSingleton<IFormValidationService, FormValidationService>();

        if (setupAction != null)
        {
            var options = new FormSetupOptions(services);
            setupAction.Invoke(options);
            services.AddSingleton(options);
        }

        return services;
    }
}
