using FormsApi.Common.Registry;
using FormsApi.Repository.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FormsApi;

public static class FormServiceCollectionExtensions
{
    public static IServiceCollection AddForms(this IServiceCollection services, Action<IFormSetupOptions>? setupAction)
    {
        services.TryAddSingleton<FormRegistry>();
        services.TryAddSingleton<RepositoryHandlerRegistry>();
        services.TryAddSingleton<RepositoryServiceBuilder>();
        services.TryAddSingleton<RepositoryTypeRegistry>();

        services.AddControllers()
            .AddApplicationPart(typeof(FormServiceCollectionExtensions).Assembly);

        if (setupAction != null)
        {
            var options = new FormSetupOptions();
            setupAction.Invoke(options);
            services.AddSingleton(options);
        }

        return services;
    }
}