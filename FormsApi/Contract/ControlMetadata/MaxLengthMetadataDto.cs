using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract.ControlMetadata;

public sealed class MaxLengthMetadataDto : IInputMetadataDto
{
    [Required]
    public required PropertyOrConstantDto Value { get; init; }
}
