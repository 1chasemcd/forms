using FormsApi.Form;
using FormsApi.Form.View;

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
            throw new InvalidFormException($"Duplicate field ids: {string.Join(", ", duplicateFields)}");
    }

    private IEnumerable<string> GetAllFieldIdsInView(BaseView view)
    {
        if (view is CombinedView combined)
            return combined.Views.SelectMany(GetAllFieldIdsInView);
        if (view is DataView data)
            return data.Fields.Select(f => f.Id);
        if (view is RepositoryGridView repoGrid)
            return repoGrid.Columns.Select(f => $"{repoGrid.RepositoryType}.{f.Id}"); // TODO this is probably wrong
        if (view is SubPropertyGridView subGrid)
            return subGrid.Columns.Select(f => $"{subGrid.SubPropertyName}.{f.Id}"); // TODO this is probably wrong
        throw new NotImplementedException($"Validation for {view.GetType()} not implemented");
    }
}
