using FormsApi.Form.View;
using FormsApi.Repository;

namespace FormsApi.Form;

public sealed class FormModel
{
    public required RepositoryType Type { get; init; }
    public required BaseView View { get; init; }
}
