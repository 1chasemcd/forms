using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FormsApi.Definition.Primitives;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(PropertyDto), nameof(PropertyDto))]
[JsonDerivedType(typeof(ConstantDto), nameof(ConstantDto))]
public abstract record PropertyOrConstantDto;
public sealed record PropertyDto : PropertyOrConstantDto
{
    public PropertyDto(string value)
    {
        Value = value;
    }

    [Required]
    public string Value { get; init; }
}
public sealed record ConstantDto : PropertyOrConstantDto
{
    public ConstantDto(object value)
    {
        Value = value;
    }
    [Required]
    public object Value { get; init; }
}
