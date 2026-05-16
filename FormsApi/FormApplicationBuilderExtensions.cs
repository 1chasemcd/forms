using FormsApi.Builder;
using FormsApi.Definition.Service;
using FormsApi.Setup;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace FormsApi;

public static class FormApplicationBuilderExtensions
{
    public static void UseForms(this IApplicationBuilder app)
    {
        MetadataBuilderService metadataBuilderService = app.ApplicationServices.GetRequiredService<MetadataBuilderService>();
        metadataBuilderService.BuildMetadataDictionary();

        FormRegistry registry = app.ApplicationServices.GetRequiredService<FormRegistry>();
        IEnumerable<FormSetupOptions> setups = app.ApplicationServices.GetServices<FormSetupOptions>();
        foreach (KeyValuePair<string, Form> formSetup in setups.SelectMany(s => s.GetForms()))
        {
            registry.AddForm(formSetup.Key, formSetup.Value);
        }
    }
}
