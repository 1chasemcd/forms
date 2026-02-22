using System.Text.Json.Serialization;
using FormsApi.Form.Json;

namespace FormsApi.Form.Primitives;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(PercentSize), "percentsize")]
[JsonDerivedType(typeof(AutoSize), "autosize")]
public abstract record FormElementSize
{
    public static implicit operator FormElementSize(int size) => new PercentSize(size);
    public static AutoSize AutoSize => new();
}

public record PercentSize(int Size = 100) : FormElementSize;
public record AutoSize : FormElementSize;
