using FormsApi.Common.Registry;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FormsApi.Common;

public static class FormServiceCollectionExtensions
{
    public static IServiceCollection AddForms(this IServiceCollection services, Action<IFormSetupOptions>? setupAction)
    {
        services.TryAddSingleton<FormRegistry>();
        if (setupAction != null)
        {
            var options = new FormSetupOptions();
            setupAction.Invoke(options);
            services.AddSingleton(options);
        }

        return services;
    }
}