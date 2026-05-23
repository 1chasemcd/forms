using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace FormsApi.Contract;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(Property), "property")]
[JsonDerivedType(typeof(Constant), "constant")]
public abstract record PropertyOrConstant;
public sealed record Property : PropertyOrConstant
{
    public Property(string value)
    {
        Value = value;
    }

    [Required]
    public string Value { get; init; }
}
public sealed record Constant : PropertyOrConstant
{
    public Constant(object value)
    {
        Value = value;
    }
    [Required]
    public object Value { get; init; }
}
