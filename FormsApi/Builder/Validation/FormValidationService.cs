using FormsApi.Definition;
using FormsApi.Definition.View;

namespace FormsApi.Builder.Validation;

internal interface IFormValidationService
{
    void Validate(FormDefinition form);
}
internal sealed class FormValidationService : IFormValidationService
{
    internal class InvalidFormException(string message) : Exception(message);
    public void Validate(FormDefinition form)
    {
        IEnumerable<string> fieldIds = GetAllFieldIdsInView(form.View);
        var fieldCounts = fieldIds.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());
        IEnumerable<string> duplicateFields = fieldCounts.Where(f => f.Value > 1).Select(f => f.Key);
        if (duplicateFields.Any())
            throw new InvalidFormException($"Duplicate field properties: {string.Join(", ", duplicateFields)}");
    }

    private IEnumerable<string> GetAllFieldIdsInView(BaseViewDefinition view)
    {
        if (view is CombinedViewDefinition combined)
            return combined.Views.SelectMany(GetAllFieldIdsInView);
        if (view is FieldViewDefinition data)
            return data.Fields.Select(f => f.Property);
        if (view is SubPropertyGridViewDefinition subGrid)
            return subGrid.Fields.Select(f => $"{subGrid.SubPropertyName}.{f.Property}").Append(subGrid.SubPropertyName);
        throw new NotImplementedException($"Validation for {view.GetType()} not implemented");
    }
}
