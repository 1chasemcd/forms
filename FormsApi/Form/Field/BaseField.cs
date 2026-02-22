using FormsApi.Form.Primitives;

namespace FormsApi.Form.Field;

public abstract record class BaseField
{
    public PropertyOrConstant? Label { get; init; }
    public PropertyOrConstant? Required { get; init; }
    public IEnumerable<string>? PropertiesToUpdateOnChange { get; init; }
    public string? MethodToRunOnChange { get; init; }
    public PropertyOrConstant? Hidden { get; init; }
    public PropertyOrConstant? Disabled { get; init; }
    public FormElementSize? Width { get; init; }
}
