using System;
using FormsApi.Builder;
using FormsApi.Builder.Validation;
using FormsApi.Common.Registry;
using FormsApi.Form;

namespace FormsApi.Setup;

internal sealed class FormSetupService(
    FormRegistry formRegistry,
    IFormValidationService validationService)
{
    public void SetupForms(IEnumerable<FormSetupOptions> setups)
    {
        foreach (KeyValuePair<string, FormBuilder> builder in setups.SelectMany(s => s.GetFormBuilders()))
        {
            FormModel form = builder.Value.Build();
            validationService.Validate(form);
            formRegistry.AddForm(builder.Key, form);
        }
    }
}
