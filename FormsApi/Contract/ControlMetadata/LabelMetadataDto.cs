using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract.ControlMetadata;

public sealed record LabelMetadataDto : BaseControlMetadataDto
{
    [Required]
    public required PropertyOrConstantDto Value { get; init; }
}
