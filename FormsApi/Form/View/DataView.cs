using FormsApi.Form.Field;

namespace FormsApi.Form.View;

public sealed record class DataView : BaseView
{
    public required IEnumerable<BaseField> Fields { get; init; }
}
