using FormsApi.Common.Registry;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace FormsApi.Common;

public static class FormApplicationBuilderExtensions
{
    public static void UseForms(this IApplicationBuilder app)
    {
        FormRegistry registry = app.ApplicationServices.GetRequiredService<FormRegistry>();
        IEnumerable<FormSetupOptions> setups = app.ApplicationServices.GetServices<FormSetupOptions>();

        foreach (FormSetupOptions setup in setups)
        {
            setup.Configure(registry);
        }
    }
}