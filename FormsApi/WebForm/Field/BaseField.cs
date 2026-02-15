using System;

namespace FormsApi.WebForm.Field;

public abstract class BaseField
{
    public PropertyOrConstant<string>? Label { get; init; }
    public string? MethodToCallOnChange { get; init; }
    public IEnumerable<string>? PropertiesToUpdateOnChange { get; init; }
    public PropertyOrConstant<bool>? Hidden { get; init; }
    public PropertyOrConstant<bool>? Disabled { get; init; }
    FormElementSize Width { get; init; }
}
