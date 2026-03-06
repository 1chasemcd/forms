using FormsApi.Form.Primitives;

namespace FormsApi.Form.Field;

public sealed record class Button : BaseField
{
    public override string Id => MethodToRunOnChange ?? Guid.NewGuid().ToString();
    public IEnumerable<string>? PropertiesToUpdateOnChange { get; init; }
    public string? MethodToRunOnChange { get; init; }
    public PropertyOrConstant? Disabled { get; init; }
}
