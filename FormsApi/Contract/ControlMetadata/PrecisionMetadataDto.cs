using System.ComponentModel.DataAnnotations;

namespace FormsApi.Contract.ControlMetadata;

public sealed class PrecisionMetadataDto : IControlMetadataDto
{
    [Required]
    public required PropertyOrConstantDto Value { get; init; }
}
