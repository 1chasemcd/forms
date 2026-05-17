using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract.ControlMetadata;

public sealed class RecalculateEventMetadataDto : IInputMetadataDto
{
    [Required]
    public required RecalculateEventDto Value { get; init; }
}
