using System.Text.Json.Serialization;

namespace FormsApi.Form.Primitives;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(Property), "property")]
[JsonDerivedType(typeof(Constant), "constant")]
public abstract record PropertyOrConstant;
public sealed record Property(string Value) : PropertyOrConstant;
public sealed record Constant(object Value) : PropertyOrConstant;
