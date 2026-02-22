using FormsApi.Common.Registry;
using FormsApi.Form.Json;
using FormsApi.Repository.Service;
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
            options.JsonSerializerOptions.TypeInfoResolverChain.Insert(0,
                new FormPolymorphicTypeResolver());
            options.JsonSerializerOptions.DefaultIgnoreCondition =
                System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            options.JsonSerializerOptions.Converters.Add(new RepositoryTypeJsonConverter());
            options.JsonSerializerOptions.Converters.Add(new FormElementSizesonConverter());
        });

        return builder;
    }

    public static IServiceCollection AddForms(this IServiceCollection services, Action<IFormSetupOptions>? setupAction)
    {
        services.TryAddSingleton<FormRegistry>();
        services.TryAddSingleton<RepositoryHandlerRegistry>();
        services.TryAddSingleton<RepositoryServiceBuilder>();
        services.TryAddSingleton<RepositoryTypeRegistry>();

        if (setupAction != null)
        {
            var options = new FormSetupOptions();
            setupAction.Invoke(options);
            services.AddSingleton(options);
        }

        return services;
    }
}
