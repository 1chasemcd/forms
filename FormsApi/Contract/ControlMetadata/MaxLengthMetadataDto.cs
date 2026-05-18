using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract.ControlMetadata;

public sealed class MaxLengthMetadataDto : IControlMetadataDto
{
    [Required]
    public required PropertyOrConstantDto Value { get; init; }
}
