using FormsApi.Form.Primitives;
using FormsApi.Form.View;

namespace FormsApi.Form;

public sealed class BaseForm
{
    public required SerializedType Type { get; init; }
    public required BaseView View { get; init; }
}
