using FormsApi.Common.Registry;
using FormsApi.Repository;
using FormsApi.Repository.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FormsApi.Common;

public static class FormServiceCollectionExtensions
{
    public static IServiceCollection AddForms(this IServiceCollection services, Action<IFormSetupOptions>? setupAction)
    {
        services.TryAddSingleton<FormRegistry>();
        services.TryAddSingleton<RepositoryHandlerRegistry>();
        services.TryAddSingleton<RepositoryServiceBuilder>();

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