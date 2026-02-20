namespace FormsApi.Form.Field;

public abstract record class BaseInput : BaseField
{
    public required string Property { get; init; }
}

public sealed record class CheckBoxInput : BaseInput;

public sealed record class TextInput : BaseInput
{
    public PropertyOrConstant<int>? MaxLength { get; init; }
}
public sealed record class TextAreaInput : BaseInput
{
    public PropertyOrConstant<int>? MaxLength { get; init; }
}

public sealed record class CurrencyInput : BaseInput
{
    public PropertyOrConstant<int>? MaxValue { get; init; }
    public PropertyOrConstant<int>? MinValue { get; init; }
}

public sealed record class NumericInput : BaseInput
{
    public PropertyOrConstant<int>? Precision { get; init; }
    public PropertyOrConstant<int>? Scale { get; init; }
    public PropertyOrConstant<int>? MaxValue { get; init; }
    public PropertyOrConstant<int>? MinValue { get; init; }
}

public sealed record class DateInput : BaseInput
{
    public PropertyOrConstant<DateOnly>? MaxValue { get; init; }
    public PropertyOrConstant<DateOnly>? MinValue { get; init; }
}

public sealed record class TimeInput : BaseInput
{
    public PropertyOrConstant<TimeOnly>? MaxValue { get; init; }
    public PropertyOrConstant<TimeOnly>? MinValue { get; init; }
}
