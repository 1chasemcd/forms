using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract.ControlMetadata;

public sealed record ScaleMetadataDto : BaseControlMetadataDto
{
    [Required]
    public required PropertyOrConstantDto Value { get; init; }
}
