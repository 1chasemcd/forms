using FormsApi.Common.Registry;
using FormsApi.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace FormsApi.Common;

public static class FormApplicationBuilderExtensions
{
    public static void UseForms(this IApplicationBuilder app)
    {
        FormRegistry formRegistry = app.ApplicationServices.GetRequiredService<FormRegistry>();
        RepositoryHandlerRegistry handlerRegistry = app.ApplicationServices.GetRequiredService<RepositoryHandlerRegistry>();

        IEnumerable<FormSetupOptions> setups = app.ApplicationServices.GetServices<FormSetupOptions>();

        foreach (FormSetupOptions setup in setups)
        {
            setup.Configure(formRegistry, handlerRegistry);
        }
    }
}