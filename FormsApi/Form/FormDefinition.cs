using FormsApi.Form.Primitives;
using FormsApi.Form.View;

namespace FormsApi.Form;

public sealed class FormDefinition
{
    public required RepositoryType Type { get; init; }
    public required BaseView View { get; init; }
}
