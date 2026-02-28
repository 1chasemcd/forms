using System.Text;
using System.Text.Json.Serialization;
using FormsApi.Form.Json;
using Microsoft.AspNetCore.WebUtilities;

namespace FormsApi.Form.Primitives;

[JsonConverter(typeof(RepositoryTypeJsonConverter))]
public record class RepositoryType(Type? type = null)
{
    public override string ToString() => type is null ? string.Empty : Encode(type);
    public static bool TryParse(string? value, out RepositoryType result)
    {
        if (!string.IsNullOrEmpty(value) && Decode(value) is Type t)
        {
            result = new RepositoryType(t);
            return true;
        }

        result = new();
        return false;
    }

    private static string Encode(Type type)
    {
        return WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(type.AssemblyQualifiedName ?? string.Empty));
    }
    public Type? GetRuntimeType()
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
