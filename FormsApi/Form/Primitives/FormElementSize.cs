using System.Text.Json.Serialization;
using FormsApi.Form.Json;

namespace FormsApi.Form.Primitives;

[JsonConverter(typeof(FormElementSizeJsonConverter))]
public abstract record FormElementSize
{
    public static implicit operator FormElementSize(int size) => new PercentSize(size);
    public static AutoSize AutoSize => new();
}

public record PercentSize(int Size = 0) : FormElementSize;
public record AutoSize : FormElementSize;
