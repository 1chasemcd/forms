using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FormsApi.Contract;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(ModelValue), "model")]
[JsonDerivedType(typeof(ConstantValue), "constant")]
public abstract record FormValueRef;
public sealed record ModelValue : FormValueRef
{
    public ModelValue(string value)
    {
        Value = value;
    }

    [Required]
    public string Value { get; init; }
}
public sealed record ConstantValue : FormValueRef
{
    public ConstantValue(object value)
    {
        Value = value;
    }
    [Required]
    public object Value { get; init; }
}
