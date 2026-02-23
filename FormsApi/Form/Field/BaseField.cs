using System.Text.Json.Serialization;
using FormsApi.Form.Primitives;

namespace FormsApi.Form.Field;

[JsonPolymorphic(
    TypeDiscriminatorPropertyName = "$type",
    UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToNearestAncestor
)]
[JsonDerivedType(typeof(Button), "button")]
[JsonDerivedType(typeof(BaseInput), "baseinput")]
[JsonDerivedType(typeof(StaticTextField), "statictextfield")]
[JsonDerivedType(typeof(CheckBoxInput), "checkboxinput")]
[JsonDerivedType(typeof(TextInput), "textinput")]
[JsonDerivedType(typeof(TextAreaInput), "textareainput")]
[JsonDerivedType(typeof(CurrencyInput), "currencyinput")]
[JsonDerivedType(typeof(NumericInput), "numericinput")]
[JsonDerivedType(typeof(DateInput), "dateinput")]
[JsonDerivedType(typeof(TimeInput), "timeinput")]
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
