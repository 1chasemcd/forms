using FormsApi.Common.Registry;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace FormsApi;

public static class FormApplicationBuilderExtensions
{
    public static void UseForms(this IApplicationBuilder app)
    {
        FormRegistry formRegistry = app.ApplicationServices.GetRequiredService<FormRegistry>();

        IEnumerable<FormSetupOptions> setups = app.ApplicationServices.GetServices<FormSetupOptions>();

        foreach (FormSetupOptions setup in setups)
        {
            formRegistry.AddForms(setup.BuildForms());
        }
    }
}
