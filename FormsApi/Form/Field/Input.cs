using System.Text.Json.Serialization;
using FormsApi.Form.Primitives;

namespace FormsApi.Form.Field;

public abstract record class BaseInput : BaseField
{
    public override string Id => Property;
    public IEnumerable<string>? PropertiesToUpdateOnChange { get; init; }
    public string? MethodToRunOnChange { get; init; }
    public PropertyOrConstant? Required { get; init; }
    public required string Property { get; init; }
    public PropertyOrConstant? Disabled { get; init; }
}

public sealed record class CheckBoxInput : BaseInput;

public sealed record class TextInput : BaseInput
{
    public PropertyOrConstant? MaxLength { get; init; }
}
public sealed record class TextAreaInput : BaseInput
{
    public PropertyOrConstant? MaxLength { get; init; }
}

public sealed record class CurrencyInput : BaseInput
{
    public PropertyOrConstant? MaxValue { get; init; }
    public PropertyOrConstant? MinValue { get; init; }
}

public sealed record class NumericInput : BaseInput
{
    public PropertyOrConstant? Precision { get; init; }
    public PropertyOrConstant? Scale { get; init; }
    public PropertyOrConstant? MaxValue { get; init; }
    public PropertyOrConstant? MinValue { get; init; }
}

public sealed record class DateInput : BaseInput
{
    public PropertyOrConstant? MaxValue { get; init; }
    public PropertyOrConstant? MinValue { get; init; }
}

public sealed record class TimeInput : BaseInput
{
    public PropertyOrConstant? MaxValue { get; init; }
    public PropertyOrConstant? MinValue { get; init; }
}
