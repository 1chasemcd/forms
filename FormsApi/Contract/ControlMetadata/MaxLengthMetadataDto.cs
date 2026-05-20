using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract.ControlMetadata;

public sealed record MaxLengthMetadataDto : BaseControlMetadataDto
{
    [Required]
    public required PropertyOrConstantDto Value { get; init; }
}
