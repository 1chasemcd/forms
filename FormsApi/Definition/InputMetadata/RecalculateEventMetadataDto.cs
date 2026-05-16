using System.ComponentModel.DataAnnotations;
using FormsApi.Definition.Primitives;

namespace FormsApi.Definition.InputMetadata;

public sealed class RecalculateEventMetadataDto : IInputMetadataDto
{
    [Required]
    public required RecalculateEventDto Value { get; init; }
}
