using FormsApi.Form.Primitives;

namespace FormsApi.Form.Field;

public sealed record class ButtonField : BaseField
{
    private readonly string _id = Guid.NewGuid().ToString();
    public override string Id => _id;
    public required OnChangeEvent OnChange { get; init; }
    public PropertyOrConstant? Disabled { get; init; }
}
