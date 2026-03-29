using System.Text;
using System.Text.Json.Serialization;
using FormsApi.Json;
using Microsoft.AspNetCore.WebUtilities;

namespace FormsApi.Definition.Primitives;

[JsonConverter(typeof(SerializedTypeJsonConverter))]
public record class SerializedType(Type type)
{
    public override string ToString() => type is null ? string.Empty : Encode(type);
    public static bool TryParse(string? value, out SerializedType result)
    {
        if (!string.IsNullOrEmpty(value) && Decode(value) is Type t)
        {
            result = new SerializedType(t);
            return true;
        }

        result = null!;
        return false;
    }

    private static string Encode(Type type)
    {
        return WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(type.AssemblyQualifiedName ?? string.Empty));
    }
    public Type GetRuntimeType()
    {
        return type;
    }

    private static Type? Decode(string encoded)
    {
        try
        {
            string assemblyQualifiedName = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(encoded));
            return Type.GetType(assemblyQualifiedName);
        }
        catch
        {
            return null;
        }
    }
}
