using FormsApi.Forms;
using FormsApi.Forms.Services;
using FormsApi.Metadata.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace FormsApi;

public static class FormApplicationBuilderExtensions
{
    public static void UseForms(this IApplicationBuilder app)
    {
        IMetadataBuilderService metadataBuilderService = app.ApplicationServices.GetRequiredService<IMetadataBuilderService>();
        metadataBuilderService.CollectMetadataDictionary();

        IFormRegistry registry = app.ApplicationServices.GetRequiredService<IFormRegistry>();
        IEnumerable<FormSetupOptions> setups = app.ApplicationServices.GetServices<FormSetupOptions>();
        foreach (KeyValuePair<string, IForm> formSetup in setups.SelectMany(s => s.GetForms()))
        {
            registry.AddForm(formSetup.Key, formSetup.Value);
        }
    }
}
