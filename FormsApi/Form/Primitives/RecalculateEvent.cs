using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FormsApi.Form.Primitives;

public sealed class RecalculateEvent
{
    [Required]
    public required SerializedType Service { get; init; }
    [Required]
    public required string Method { get; init; }
    [Required]
    public bool DontSendModel { get; init; }
}
