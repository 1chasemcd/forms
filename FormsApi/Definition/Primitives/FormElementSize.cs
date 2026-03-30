using System.Text.Json.Serialization;
using FormsApi.Json;

namespace FormsApi.Definition.Primitives;

[JsonConverter(typeof(FormElementSizeJsonConverter))]
public abstract record FormElementSize
{
    public static implicit operator FormElementSize(int size) => new NumericSize(size);
    public static AutoSize AutoSize => new();
}

public record NumericSize(int Size = 0) : FormElementSize;
public record AutoSize : FormElementSize;
