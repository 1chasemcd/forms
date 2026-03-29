using System.Text.Json.Serialization;
using FormsApi.Definition.Primitives;

namespace FormsApi.Definition.Field;

[JsonPolymorphic(
    TypeDiscriminatorPropertyName = "$type",
    UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToNearestAncestor
)]
[JsonDerivedType(typeof(StaticTextDefinition), "statictext")]
[JsonDerivedType(typeof(ButtonDefinition), "button")]
[JsonDerivedType(typeof(CheckBoxInputDefinition), "checkboxinput")]
[JsonDerivedType(typeof(TextInputDefinition), "textinput")]
[JsonDerivedType(typeof(TextAreaInputDefinition), "textareainput")]
[JsonDerivedType(typeof(CurrencyInputDefinition), "currencyinput")]
[JsonDerivedType(typeof(NumericInputDefinition), "numericinput")]
[JsonDerivedType(typeof(DateInputDefinition), "dateinput")]
[JsonDerivedType(typeof(TimeInputDefinition), "timeinput")]
public interface IFieldDefinition
{
    PropertyOrConstant Label { get; init; }
    PropertyOrConstant? Hidden { get; init; }
    FormElementSize? Width { get; init; }
}