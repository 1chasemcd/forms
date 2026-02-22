using System.Text.Json.Serialization;
using FormsApi.Form.Json;

namespace FormsApi.Form.Primitives;

[JsonConverter(typeof(RepositoryTypeJsonConverter))]
public readonly record struct RepositoryType(string TypeId)
{
    public override string ToString() => TypeId;
    public static bool TryParse(string? value, out RepositoryType result)
    {
        if (!string.IsNullOrEmpty(value))
        {
            result = new RepositoryType(value);
            return true;
        }

        result = default;
        return false;
    }
}
