using FormsApi.Setup;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace FormsApi;

public static class FormApplicationBuilderExtensions
{
    public static void UseForms(this IApplicationBuilder app)
    {
        FormSetupService setupService = app.ApplicationServices.GetRequiredService<FormSetupService>();
        IEnumerable<FormSetupOptions> setups = app.ApplicationServices.GetServices<FormSetupOptions>();
        setupService.SetupForms(setups);
    }
}
