using FormsApi.Common.Registry;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace FormsApi;

public static class FormApplicationBuilderExtensions
{
    public static void UseForms(this IApplicationBuilder app)
    {
        FormRegistry formRegistry = app.ApplicationServices.GetRequiredService<FormRegistry>();
        RepositoryRegistry repositoryRegistry = app.ApplicationServices.GetRequiredService<RepositoryRegistry>();

        IEnumerable<FormSetupOptions> setups = app.ApplicationServices.GetServices<FormSetupOptions>();

        foreach (FormSetupOptions setup in setups)
        {
            setup.Configure(formRegistry, repositoryRegistry);
        }
    }
}
