using System;
using System.Text.Json.Serialization;

namespace FormsApi.Definition.Field;


[JsonPolymorphic(
    TypeDiscriminatorPropertyName = "$type",
    UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToNearestAncestor
)]
[JsonDerivedType(typeof(CheckBoxInputDefinition), "checkboxinput")]
[JsonDerivedType(typeof(TextInputDefinition), "textinput")]
[JsonDerivedType(typeof(TextAreaInputDefinition), "textareainput")]
[JsonDerivedType(typeof(CurrencyInputDefinition), "currencyinput")]
[JsonDerivedType(typeof(NumericInputDefinition), "numericinput")]
[JsonDerivedType(typeof(DateInputDefinition), "dateinput")]
[JsonDerivedType(typeof(TimeInputDefinition), "timeinput")]
public interface IModelableField
{
    string Property { get; init; }
}
