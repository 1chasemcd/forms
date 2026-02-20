namespace FormsApi.Form.Field;

public abstract record class BaseField
{
    public PropertyOrConstant<string>? Label { get; init; }
    public PropertyOrConstant<bool>? Required { get; init; }
    public IEnumerable<string>? PropertiesToUpdateOnChange { get; init; }
    public string? MethodToRunOnChange { get; init; }
    public PropertyOrConstant<bool>? Hidden { get; init; }
    public PropertyOrConstant<bool>? Disabled { get; init; }
    public FormElementSize Width { get; init; }
}
